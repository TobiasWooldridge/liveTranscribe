using EchoSharp.Abstractions.SpeechTranscription;
using EchoSharp.Onnx.SileroVad;
using EchoSharp.SpeechTranscription;
using EchoSharp.Whisper.net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Whisper.net;
using Whisper.net.Ggml;

namespace liveTranscribe
{
    public partial class Form1 : Form
    {
        private static readonly Uri SileroVadUri = new("https://github.com/sandrohanea/silero-vad/raw/refs/tags/v1/src/silero_vad/data/silero_vad.onnx");
        private static readonly string ModelsDirectory = Path.Combine(AppContext.BaseDirectory, "models");

        private readonly List<ModelOption> modelOptions = new()
        {
            new ModelOption("Tiny (fastest, lowest quality)", GgmlType.Tiny, "ggml-Tiny.bin"),
            new ModelOption("Base", GgmlType.Base, "ggml-Base.bin"),
            new ModelOption("Small", GgmlType.Small, "ggml-Small.bin"),
            new ModelOption("Medium (default)", GgmlType.Medium, "ggml-Medium.bin"),
            new ModelOption("Large V3 (highest quality)", GgmlType.LargeV3, "ggml-LargeV3.bin")
        };

        private readonly List<LanguageOption> languageOptions = new()
        {
            new LanguageOption("English (United States)", new CultureInfo("en-US")),
            new LanguageOption("English (United Kingdom)", new CultureInfo("en-GB")),
            new LanguageOption("Spanish (Spain)", new CultureInfo("es-ES")),
            new LanguageOption("Spanish (Latin America)", new CultureInfo("es-MX")),
            new LanguageOption("French", new CultureInfo("fr-FR")),
            new LanguageOption("German", new CultureInfo("de-DE")),
            new LanguageOption("Italian", new CultureInfo("it-IT")),
            new LanguageOption("Portuguese (Brazil)", new CultureInfo("pt-BR")),
            new LanguageOption("Portuguese (Portugal)", new CultureInfo("pt-PT")),
            new LanguageOption("Chinese (Mandarin)", new CultureInfo("zh-CN")),
            new LanguageOption("Japanese", new CultureInfo("ja-JP")),
            new LanguageOption("Hindi", new CultureInfo("hi-IN"))
        };

        private WaspiLoopbackAudioSource? loopbackAudioSource;
        private Task? transcriptionTask;
        private CancellationTokenSource transcriptionCancellation = new();
        private SileroVadDetectorFactory? vadDetectorFactory;
        private WhisperFactory? whisperFactory;
        private WhisperSpeechTranscriptorFactory? whisperTranscriptorFactory;
        private EchoSharpRealtimeTranscriptorFactory? realtimeFactory;
        private IRealtimeSpeechTranscriptor? realtimeTranscriptor;
        private string? currentModelPath;

        public Form1()
        {
            InitializeComponent();
            ConfigureOptionControls();
        }

        private void ConfigureOptionControls()
        {
            modelComboBox.DisplayMember = nameof(ModelOption.DisplayName);
            modelComboBox.ValueMember = nameof(ModelOption.ModelType);
            modelComboBox.DataSource = modelOptions;
            modelComboBox.SelectedItem = modelOptions.First(option => option.ModelType == GgmlType.Medium);

            languageComboBox.DisplayMember = nameof(LanguageOption.DisplayName);
            languageComboBox.ValueMember = nameof(LanguageOption.Culture);
            languageComboBox.DataSource = languageOptions;
            languageComboBox.SelectedItem = languageOptions.First();

            UpdateLanguageControls();
            UpdateSamplingControls();
        }

        private void UpdateLanguageControls()
        {
            languageComboBox.Enabled = !autoDetectLanguageCheckBox.Checked;
        }

        private void UpdateSamplingControls()
        {
            bestOfNumericUpDown.Enabled = greedyRadioButton.Checked;
            beamSizeNumericUpDown.Enabled = beamSearchRadioButton.Checked;
            patienceNumericUpDown.Enabled = beamSearchRadioButton.Checked;
        }

        private async Task EnsureVadDetectorAsync()
        {
            Directory.CreateDirectory(ModelsDirectory);
            if (vadDetectorFactory != null)
            {
                return;
            }

            var vadPath = Path.Combine(ModelsDirectory, "silero_vad.onnx");
            if (!File.Exists(vadPath))
            {
                using var client = new WebClient();
                await client.DownloadFileTaskAsync(SileroVadUri, vadPath);
            }

            vadDetectorFactory = new SileroVadDetectorFactory(new SileroVadOptions(vadPath)
            {
                Threshold = 0.5f,
                ThresholdGap = 0.15f
            });
        }

        private static async Task EnsureModelFileAsync(GgmlType modelType, string destinationPath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);
            if (File.Exists(destinationPath))
            {
                return;
            }

            using var modelStream = await WhisperGgmlDownloader.Default.GetGgmlModelAsync(modelType);
            using var fileWriter = File.Open(destinationPath, FileMode.Create, FileAccess.Write, FileShare.Read);
            await modelStream.CopyToAsync(fileWriter);
        }

        private async Task EnsureRealtimeTranscriptorAsync()
        {
            await EnsureVadDetectorAsync();

            var selectedModel = (ModelOption)(modelComboBox.SelectedItem ?? modelOptions[0]);
            var modelPath = Path.Combine(ModelsDirectory, selectedModel.FileName);
            await EnsureModelFileAsync(selectedModel.ModelType, modelPath);

            if (!string.Equals(currentModelPath, modelPath, StringComparison.OrdinalIgnoreCase))
            {
                whisperFactory?.Dispose();
                whisperFactory = WhisperFactory.FromPath(modelPath);
                currentModelPath = modelPath;
            }

            var builder = whisperFactory!.CreateBuilder()
                .WithThreads(Math.Max(1, Environment.ProcessorCount))
                .WithTemperature((float)temperatureNumericUpDown.Value)
                .WithNoSpeechThreshold((float)noSpeechNumericUpDown.Value)
                .WithLogProbThreshold((float)logProbNumericUpDown.Value);

            if (translateCheckBox.Checked)
            {
                builder.WithTranslate();
            }

            if (autoDetectLanguageCheckBox.Checked)
            {
                builder.WithLanguageDetection();
            }
            else if (languageComboBox.SelectedItem is LanguageOption { Culture: { } culture })
            {
                builder.WithLanguage(culture.TwoLetterISOLanguageName);
            }

            if (beamSearchRadioButton.Checked)
            {
                builder.WithBeamSearchSamplingStrategy()
                       .WithBeamSize((int)beamSizeNumericUpDown.Value)
                       .WithPatience((float)patienceNumericUpDown.Value);
            }
            else
            {
                builder.WithGreedySamplingStrategy()
                       .WithBestOf((int)bestOfNumericUpDown.Value);
            }

            whisperTranscriptorFactory?.Dispose();
            whisperTranscriptorFactory = new WhisperSpeechTranscriptorFactory(builder);

            realtimeFactory = new EchoSharpRealtimeTranscriptorFactory(
                whisperTranscriptorFactory,
                vadDetectorFactory!,
                new EchoSharpRealtimeOptions
                {
                    ConcatenateSegmentsToPrompt = false
                });

            var language = (languageComboBox.SelectedItem as LanguageOption)?.Culture ?? new CultureInfo("en-US");

            realtimeTranscriptor = realtimeFactory.Create(new RealtimeSpeechTranscriptorOptions
            {
                AutodetectLanguageOnce = autoDetectLanguageCheckBox.Checked,
                IncludeSpeechRecogizingEvents = true,
                RetrieveTokenDetails = true,
                LanguageAutoDetect = autoDetectLanguageCheckBox.Checked,
                Language = language
            });
        }

        private async void startButton_Click(object? sender, EventArgs e)
        {
            if (loopbackAudioSource != null)
            {
                return;
            }

            startButton.Enabled = false;
            stopButton.Enabled = false;
            statusLabel.Text = "Status: Preparing...";
            transcriptTextBox.Clear();

            try
            {
                await EnsureRealtimeTranscriptorAsync();

                loopbackAudioSource = new WaspiLoopbackAudioSource();
                loopbackAudioSource.StartRecording();

                transcriptionCancellation = new CancellationTokenSource();
                stopButton.Enabled = true;
                statusLabel.Text = "Status: Listening...";

                transcriptionTask = ShowTranscriptAsync(transcriptionCancellation.Token);
            }
            catch (Exception ex)
            {
                statusLabel.Text = "Status: Error";
                MessageBox.Show(this, $"Unable to start transcription: {ex.Message}", "Transcription error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                loopbackAudioSource?.Dispose();
                loopbackAudioSource = null;
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
        }

        private async void stopButton_Click(object? sender, EventArgs e)
        {
            stopButton.Enabled = false;

            if (loopbackAudioSource == null)
            {
                startButton.Enabled = true;
                return;
            }

            try
            {
                loopbackAudioSource.StopRecording();
            }
            catch
            {
            }
            finally
            {
                loopbackAudioSource.Dispose();
                loopbackAudioSource = null;
            }

            transcriptionCancellation.Cancel();

            if (transcriptionTask != null)
            {
                try
                {
                    await transcriptionTask;
                }
                catch (OperationCanceledException)
                {
                }
            }

            transcriptionCancellation.Dispose();
            transcriptionCancellation = new CancellationTokenSource();
            transcriptionTask = null;

            statusLabel.Text = "Status: Stopped";
            startButton.Enabled = true;
        }

        private async Task ShowTranscriptAsync(CancellationToken cancellationToken)
        {
            if (realtimeTranscriptor == null || loopbackAudioSource == null)
            {
                return;
            }

            try
            {
                await foreach (var transcription in realtimeTranscriptor.TranscribeAsync(loopbackAudioSource, cancellationToken))
                {
                    switch (transcription)
                    {
                        case RealtimeSegmentRecognized recognized:
                            AppendRecognizedSegment(recognized);
                            break;
                        case RealtimeSegmentRecognizing recognizing:
                            UpdateRecognizingStatus(recognizing);
                            break;
                        case RealtimeSessionStarted started:
                            UpdateStatus($"Status: Session {started.SessionId} started");
                            break;
                        case RealtimeSessionStopped:
                            UpdateStatus("Status: Listening...");
                            break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                UpdateStatus("Status: Stopped");
            }
            catch (Exception ex)
            {
                UpdateStatus("Status: Error");
                BeginInvoke(new Action(() =>
                {
                    MessageBox.Show(this, $"Transcription failed: {ex.Message}", "Transcription error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
            }
            finally
            {
                BeginInvoke(new Action(() => stopButton.Enabled = false));
                BeginInvoke(new Action(() => startButton.Enabled = true));
            }
        }

        private void AppendRecognizedSegment(RealtimeSegmentRecognized recognized)
        {
            var text = recognized.Segment.Text?.Trim();
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            BeginInvoke(new Action(() =>
            {
                transcriptTextBox.AppendText(text + Environment.NewLine);
                statusLabel.Text = "Status: Listening...";
            }));
        }

        private void UpdateRecognizingStatus(RealtimeSegmentRecognizing recognizing)
        {
            var text = recognizing.Segment.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            UpdateStatus($"Status: Hearing... {text.Trim()}");
        }

        private void UpdateStatus(string status)
        {
            if (!IsHandleCreated)
            {
                return;
            }

            BeginInvoke(new Action(() => statusLabel.Text = status));
        }

        private void autoDetectLanguageCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            UpdateLanguageControls();
        }

        private void samplingRadioButton_CheckedChanged(object? sender, EventArgs e)
        {
            UpdateSamplingControls();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            CleanupResources();
        }

        private void CleanupResources()
        {
            try
            {
                transcriptionCancellation.Cancel();
            }
            catch
            {
            }

            transcriptionTask = null;
            transcriptionCancellation.Dispose();

            loopbackAudioSource?.Dispose();
            loopbackAudioSource = null;

            if (realtimeTranscriptor is IDisposable disposableTranscriptor)
            {
                disposableTranscriptor.Dispose();
            }

            (realtimeFactory as IDisposable)?.Dispose();
            whisperTranscriptorFactory?.Dispose();
            whisperFactory?.Dispose();
            (vadDetectorFactory as IDisposable)?.Dispose();
        }

        private sealed record ModelOption(string DisplayName, GgmlType ModelType, string FileName);

        private sealed record LanguageOption(string DisplayName, CultureInfo? Culture);
    }
}
