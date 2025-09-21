using EchoSharp.Abstractions.SpeechTranscription;
using EchoSharp.Onnx.SileroVad;
using EchoSharp.SpeechTranscription;
using EchoSharp.Whisper.net;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Whisper.net;
using Whisper.net.Ggml;

namespace liveTranscribe
{
    public partial class Form1 : Form
    {
        private const string ModelsFolderName = "models";
        private const string SileroVadFileName = "silero_vad.onnx";
        private static readonly Uri SileroVadUri = new("https://github.com/sandrohanea/silero-vad/raw/refs/tags/v1/src/silero_vad/data/silero_vad.onnx");
        private static readonly HttpClient HttpClient = new() { Timeout = TimeSpan.FromMinutes(10) };

        private static readonly IReadOnlyDictionary<GgmlType, string> ModelFileNames = new Dictionary<GgmlType, string>
        {
            [GgmlType.Tiny] = "ggml-tiny.bin",
            [GgmlType.Base] = "ggml-base.bin",
            [GgmlType.Small] = "ggml-small.bin",
            [GgmlType.Medium] = "ggml-medium.bin",
            [GgmlType.LargeV1] = "ggml-large.bin",
            [GgmlType.LargeV2] = "ggml-large-v2.bin",
            [GgmlType.LargeV3] = "ggml-large-v3.bin"
        };

        private readonly List<WhisperModelOption> modelOptions = new()
        {
            new WhisperModelOption("Tiny (fastest)", GgmlType.Tiny),
            new WhisperModelOption("Base", GgmlType.Base),
            new WhisperModelOption("Small", GgmlType.Small),
            new WhisperModelOption("Medium", GgmlType.Medium),
            new WhisperModelOption("Large v2", GgmlType.LargeV2),
            new WhisperModelOption("Large v3 (highest quality)", GgmlType.LargeV3)
        };

        private readonly SemaphoreSlim initializationSemaphore = new(1, 1);
        private readonly StringBuilder transcriptBuilder = new();

        private WaspiLoopbackAudioSource? waveloop;
        private Task? showTranscriptTask;
        private CancellationTokenSource? transcriptionCts;
        private IRealtimeSpeechTranscriptor? realTimeTranscriptor;
        private SileroVadDetectorFactory? vadDetectorFactory;
        private WhisperFactory? factory;
        private WhisperSpeechTranscriptorFactory? speechTranscriptorFactory;
        private EchoSharpRealtimeTranscriptorFactory? realTimeFactory;
        private WhisperBuilderOptions? loadedOptions;

        public Form1()
        {
            InitializeComponent();
            InitializeUiDefaults();
        }

        private void InitializeUiDefaults()
        {
            modelComboBox.DisplayMember = nameof(WhisperModelOption.DisplayName);
            modelComboBox.ValueMember = nameof(WhisperModelOption.ModelType);
            modelComboBox.DataSource = modelOptions;
            modelComboBox.SelectedIndex = modelOptions.FindIndex(m => m.ModelType == GgmlType.Medium);

            temperatureNumericUpDown.DecimalPlaces = 2;
            temperatureNumericUpDown.Increment = 0.05M;
            temperatureNumericUpDown.Minimum = 0;
            temperatureNumericUpDown.Maximum = 1;
            temperatureNumericUpDown.Value = 0;

            temperatureStepNumericUpDown.DecimalPlaces = 2;
            temperatureStepNumericUpDown.Increment = 0.05M;
            temperatureStepNumericUpDown.Minimum = 0;
            temperatureStepNumericUpDown.Maximum = 1;
            temperatureStepNumericUpDown.Value = 0.20M;

            beamSizeNumericUpDown.Minimum = 1;
            beamSizeNumericUpDown.Maximum = 16;
            beamSizeNumericUpDown.Value = 5;

            bestOfNumericUpDown.Minimum = 1;
            bestOfNumericUpDown.Maximum = 10;
            bestOfNumericUpDown.Value = 3;

            beamSearchCheckBox.Checked = true;
            translateCheckBox.Checked = false;
            autoDetectLanguageCheckBox.Checked = false;
            languageTextBox.Text = "en-US";

            statusLabel.Text = "Idle";
            stopButton.Enabled = false;
        }

        private async void startButton_Click(object sender, EventArgs e)
        {
            if (waveloop != null)
            {
                return;
            }

            var options = GetRequestedOptions();
            bool started = false;

            try
            {
                startButton.Enabled = false;
                stopButton.Enabled = false;
                ClearTranscript();
                UpdateStatus("Preparing models...");

                await EnsureRealtimeTranscriptorAsync(options).ConfigureAwait(true);

                transcriptionCts = new CancellationTokenSource();
                waveloop = new WaspiLoopbackAudioSource();
                waveloop.StartRecording();

                stopButton.Enabled = true;
                started = true;

                showTranscriptTask = ShowTranscriptAsync(transcriptionCts.Token);
            }
            catch (CultureNotFoundException)
            {
                ShowError($"The language code '{options.Language}' is not valid.", "Invalid language");
            }
            catch (Exception ex)
            {
                ShowError($"Failed to start transcription: {ex.Message}");
            }
            finally
            {
                if (!started)
                {
                    startButton.Enabled = true;
                    stopButton.Enabled = false;
                    transcriptionCts?.Dispose();
                    transcriptionCts = null;
                    waveloop?.Dispose();
                    waveloop = null;
                }
            }
        }

        private async void stopButton_Click(object sender, EventArgs e)
        {
            stopButton.Enabled = false;
            await StopTranscriptionAsync().ConfigureAwait(true);
        }

        private async Task StopTranscriptionAsync()
        {
            var cts = transcriptionCts;
            transcriptionCts = null;
            cts?.Cancel();

            var currentLoop = waveloop;
            waveloop = null;
            currentLoop?.StopRecording();
            currentLoop?.Dispose();

            if (cts != null)
            {
                cts.Dispose();
            }

            if (showTranscriptTask != null)
            {
                try
                {
                    await showTranscriptTask.ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    // Expected when cancellation is requested.
                }
                catch (Exception ex)
                {
                    ShowError($"Transcription stopped with error: {ex.Message}", "Transcription error");
                }
                finally
                {
                    showTranscriptTask = null;
                }
            }

            UpdateStatus("Idle");
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    startButton.Enabled = true;
                    stopButton.Enabled = false;
                }));
            }
            else
            {
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
        }

        private WhisperBuilderOptions GetRequestedOptions()
        {
            var selectedModel = modelComboBox.SelectedItem as WhisperModelOption ?? modelOptions[0];

            return new WhisperBuilderOptions(
                selectedModel.ModelType,
                translateCheckBox.Checked,
                autoDetectLanguageCheckBox.Checked,
                languageTextBox.Text.Trim(),
                beamSearchCheckBox.Checked,
                (int)beamSizeNumericUpDown.Value,
                (int)bestOfNumericUpDown.Value,
                (float)temperatureNumericUpDown.Value,
                (float)temperatureStepNumericUpDown.Value);
        }

        private async Task EnsureRealtimeTranscriptorAsync(WhisperBuilderOptions options)
        {
            await initializationSemaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                if (loadedOptions == options && realTimeTranscriptor != null)
                {
                    return;
                }

                await EnsureSileroVadFactoryAsync().ConfigureAwait(false);
                var modelPath = await EnsureModelAvailableAsync(options.ModelType).ConfigureAwait(false);

                DisposeTranscriptionPipeline();

                factory = WhisperFactory.FromPath(modelPath);
                var builder = factory.CreateBuilder();

                builder.WithThreads(Environment.ProcessorCount);
                builder.WithTemperature(options.Temperature);
                if (options.TemperatureIncrement > 0)
                {
                    builder.WithTemperatureInc(options.TemperatureIncrement);
                }

                if (options.Translate)
                {
                    builder.WithTranslate();
                }

                if (options.AutoDetectLanguage)
                {
                    builder.WithLanguageDetection();
                }

                if (options.UseBeamSearch)
                {
                    var beamBuilder = builder.WithBeamSearchSamplingStrategy();
                    beamBuilder.WithBeamSize(options.BeamSize);
                }
                else
                {
                    var greedyBuilder = builder.WithGreedySamplingStrategy();
                    greedyBuilder.WithBestOf(options.BestOf);
                }

                speechTranscriptorFactory = new WhisperSpeechTranscriptorFactory(builder);
                realTimeFactory = new EchoSharpRealtimeTranscriptorFactory(
                    speechTranscriptorFactory,
                    vadDetectorFactory!,
                    echoSharpOptions: new EchoSharpRealtimeOptions
                    {
                        ConcatenateSegmentsToPrompt = false
                    });

                var cultureInfo = ResolveLanguage(options);
                if (cultureInfo != CultureInfo.InvariantCulture)
                {
                    builder.WithLanguage(cultureInfo.Name);
                }

                realTimeTranscriptor = realTimeFactory.Create(new RealtimeSpeechTranscriptorOptions()
                {
                    AutodetectLanguageOnce = options.AutoDetectLanguage,
                    IncludeSpeechRecogizingEvents = true,
                    RetrieveTokenDetails = true,
                    LanguageAutoDetect = options.AutoDetectLanguage,
                    Language = cultureInfo
                });

                loadedOptions = options;
                UpdateStatus("Models ready");
            }
            finally
            {
                initializationSemaphore.Release();
            }
        }

        private async Task EnsureSileroVadFactoryAsync()
        {
            if (vadDetectorFactory != null)
            {
                return;
            }

            var modelsDirectory = GetModelsDirectory();
            Directory.CreateDirectory(modelsDirectory);
            var vadPath = Path.Combine(modelsDirectory, SileroVadFileName);

            if (!File.Exists(vadPath))
            {
                UpdateStatus("Downloading VAD model...");
                using var response = await HttpClient.GetAsync(SileroVadUri, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                await using var fileStream = File.Create(vadPath);
                await stream.CopyToAsync(fileStream).ConfigureAwait(false);
            }

            vadDetectorFactory = new SileroVadDetectorFactory(new SileroVadOptions(vadPath)
            {
                Threshold = 0.6f,
                ThresholdGap = 0.1f
            });
        }

        private async Task<string> EnsureModelAvailableAsync(GgmlType modelType)
        {
            if (!ModelFileNames.TryGetValue(modelType, out var fileName))
            {
                throw new InvalidOperationException($"Model file name not configured for {modelType}.");
            }

            var modelsDirectory = GetModelsDirectory();
            Directory.CreateDirectory(modelsDirectory);
            var modelPath = Path.Combine(modelsDirectory, fileName);

            if (!File.Exists(modelPath))
            {
                UpdateStatus($"Downloading {GetModelDisplayName(modelType)} model...");
                using var modelStream = await WhisperGgmlDownloader.Default.GetGgmlModelAsync(modelType).ConfigureAwait(false);
                await using var fileWriter = File.Create(modelPath);
                await modelStream.CopyToAsync(fileWriter).ConfigureAwait(false);
            }

            return modelPath;
        }

        private static string GetModelsDirectory() => Path.Combine(AppContext.BaseDirectory, ModelsFolderName);

        private string GetModelDisplayName(GgmlType type) => modelOptions.FirstOrDefault(m => m.ModelType == type)?.DisplayName ?? type.ToString();

        private CultureInfo ResolveLanguage(WhisperBuilderOptions options)
        {
            if (options.AutoDetectLanguage)
            {
                if (string.IsNullOrWhiteSpace(options.Language))
                {
                    return CultureInfo.InvariantCulture;
                }

                return CultureInfo.GetCultureInfo(options.Language);
            }

            var languageCode = string.IsNullOrWhiteSpace(options.Language) ? "en-US" : options.Language;
            return CultureInfo.GetCultureInfo(languageCode);
        }

        private async Task ShowTranscriptAsync(CancellationToken cancellationToken)
        {
            if (realTimeTranscriptor == null || waveloop == null)
            {
                return;
            }

            try
            {
                UpdateStatus("Listening...");

                await foreach (var transcription in realTimeTranscriptor
                    .TranscribeAsync(waveloop, cancellationToken)
                    .ConfigureAwait(false))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    switch (transcription)
                    {
                        case RealtimeSegmentRecognized recognized:
                            AppendTranscript(recognized.Segment.Text);
                            UpdateStatus($"Recognized segment ({recognized.Segment.Text.Length} chars)");
                            break;
                        case RealtimeSegmentRecognizing:
                            UpdateStatus("Recognizing...");
                            break;
                        case RealtimeSessionStarted sessionStarted:
                            UpdateStatus($"Session started ({sessionStarted.SessionId})");
                            break;
                        case RealtimeSessionStopped sessionStopped:
                            UpdateStatus($"Session stopped ({sessionStopped.SessionId})");
                            break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                UpdateStatus("Transcription cancelled");
            }
            catch (Exception ex)
            {
                ShowError($"Realtime transcription failed: {ex.Message}", "Transcription error");
            }
        }

        private void AppendTranscript(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(AppendTranscript), text);
                return;
            }

            var trimmed = text.Trim();
            if (trimmed.Length == 0)
            {
                return;
            }

            if (transcriptBuilder.Length > 0 && !char.IsWhiteSpace(transcriptBuilder[^1]))
            {
                transcriptBuilder.Append(' ');
            }

            transcriptBuilder.Append(trimmed);
            transcriptTextBox.Text = transcriptBuilder.ToString();
            transcriptTextBox.SelectionStart = transcriptTextBox.TextLength;
            transcriptTextBox.ScrollToCaret();
        }

        private void ClearTranscript()
        {
            transcriptBuilder.Clear();
            transcriptTextBox.Clear();
        }

        private void UpdateStatus(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(UpdateStatus), message);
                return;
            }

            statusLabel.Text = message;
        }

        private void ShowError(string message, string caption = "Error")
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => ShowError(message, caption)));
                return;
            }

            MessageBox.Show(this, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            StopTranscriptionAsync().GetAwaiter().GetResult();
            DisposeTranscriptionPipeline();
            if (vadDetectorFactory is IDisposable disposableVad)
            {
                disposableVad.Dispose();
            }

            base.OnFormClosed(e);
        }

        private void DisposeTranscriptionPipeline()
        {
            realTimeTranscriptor?.Dispose();
            realTimeTranscriptor = null;

            realTimeFactory?.Dispose();
            realTimeFactory = null;

            speechTranscriptorFactory?.Dispose();
            speechTranscriptorFactory = null;

            factory?.Dispose();
            factory = null;
        }

        private sealed record WhisperModelOption(string DisplayName, GgmlType ModelType);

        private sealed record WhisperBuilderOptions(
            GgmlType ModelType,
            bool Translate,
            bool AutoDetectLanguage,
            string Language,
            bool UseBeamSearch,
            int BeamSize,
            int BestOf,
            float Temperature,
            float TemperatureIncrement);
    }
}
