using EchoSharp.Abstractions.SpeechTranscription;
using EchoSharp.Onnx.SileroVad;
using EchoSharp.SpeechTranscription;
using EchoSharp.Whisper.net;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Whisper.net;
using Whisper.net.Ggml;
namespace liveTranscribe
{
    public partial class Form1 : Form
    {
        WaspiLoopbackAudioSource? waveloop;
        Task? showTranscriptTask;
        private IRealtimeSpeechTranscriptor? realTimeTranscriptor;
        private SileroVadDetectorFactory? vadDetectorFactory;
        private WhisperFactory? factory;
        private WhisperSpeechTranscriptorFactory? speechTranscriptorFactory;
        private EchoSharpRealtimeTranscriptorFactory? realTimeFactory;
        private CancellationTokenSource? token;
        private WhisperConfiguration? currentConfiguration;

        private readonly IReadOnlyList<ModelOption> modelOptions = new List<ModelOption>
        {
            new("Tiny", GgmlType.Tiny),
            new("Base", GgmlType.Base),
            new("Small", GgmlType.Small),
            new("Medium", GgmlType.Medium),
            new("Large V2", GgmlType.LargeV2)
        };

        public Form1()
        {
            InitializeComponent();
            InitializeConfigurationControls();
        }

        private void InitializeConfigurationControls()
        {
            statusLabel.Text = "Status: Idle";

            modelComboBox.DisplayMember = nameof(ModelOption.DisplayName);
            modelComboBox.ValueMember = nameof(ModelOption.ModelType);
            foreach (var option in modelOptions)
            {
                modelComboBox.Items.Add(option);
            }
            modelComboBox.SelectedItem = modelOptions.First(option => option.ModelType == GgmlType.Medium);

            threadsNumeric.Value = Math.Max(1, Environment.ProcessorCount - 1);
            temperatureNumeric.Value = 0.2M;
            beamSizeNumeric.Value = 5;
            bestOfNumeric.Value = 1;
            vadThresholdNumeric.Value = 0.5M;
            vadGapNumeric.Value = 0.15M;
            translateCheckBox.Checked = false;
            autoDetectLanguageCheckBox.Checked = false;
            autodetectOnceCheckBox.Checked = false;
            partialResultsCheckBox.Checked = true;
            tokenDetailsCheckBox.Checked = true;
            languageTextBox.Text = CultureInfo.CurrentCulture.Name;
        }

        private async Task EnsureTranscriptionConfiguredAsync()
        {
            var configuration = BuildConfigurationFromUi();

            if (realTimeTranscriptor != null && currentConfiguration.HasValue && currentConfiguration.Value.Equals(configuration))
            {
                return;
            }

            DisposeFactories();

            statusLabel.Text = "Status: Loading voice activity detector...";
            if (!Directory.Exists("models"))
            {
                Directory.CreateDirectory("models");
            }
            var sileroOnnxPath = "models/silero_vad.onnx";
            if (!File.Exists(sileroOnnxPath))
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(new Uri("https://github.com/sandrohanea/silero-vad/raw/refs/tags/v1/src/silero_vad/data/silero_vad.onnx"), sileroOnnxPath);
                }
            }
            vadDetectorFactory = new SileroVadDetectorFactory(new SileroVadOptions(sileroOnnxPath)
            {
                Threshold = configuration.VadThreshold,
                ThresholdGap = configuration.VadThresholdGap,
            });

            var ggmlModelPath = $"models/ggml-{configuration.ModelType}.bin";
            if (!File.Exists(ggmlModelPath))
            {
                statusLabel.Text = $"Status: Downloading {configuration.ModelType} model...";
                using var modelStream = await WhisperGgmlDownloader.Default.GetGgmlModelAsync(configuration.ModelType);
                using var fileWriter = File.OpenWrite(ggmlModelPath);
                await modelStream.CopyToAsync(fileWriter);
            }

            statusLabel.Text = "Status: Initializing Whisper...";
            factory = WhisperFactory.FromPath(ggmlModelPath);
            var processorBuilder = factory.CreateBuilder();

            if (configuration.Threads > 0)
            {
                processorBuilder = processorBuilder.WithThreads(configuration.Threads);
            }

            processorBuilder = processorBuilder.WithTemperature(configuration.Temperature);

            if (!string.IsNullOrWhiteSpace(configuration.Prompt))
            {
                processorBuilder = processorBuilder.WithPrompt(configuration.Prompt);
            }

            if (configuration.Translate)
            {
                processorBuilder = processorBuilder.WithTranslate();
            }

            if (configuration.UseLanguageDetection)
            {
                processorBuilder = processorBuilder.WithLanguageDetection();
            }
            else
            {
                processorBuilder = processorBuilder.WithLanguage(configuration.LanguageCode);
            }

            if (configuration.BeamSize > 1)
            {
                var beamBuilder = processorBuilder.WithBeamSearchSamplingStrategy();
                if (beamBuilder is BeamSearchSamplingStrategyBuilder beamSearchSamplingStrategyBuilder)
                {
                    beamSearchSamplingStrategyBuilder.WithBeamSize(configuration.BeamSize);
                }
                processorBuilder = beamBuilder.ParentBuilder;
            }
            else
            {
                var greedyBuilder = processorBuilder.WithGreedySamplingStrategy();
                if (greedyBuilder is GreedySamplingStrategyBuilder greedySamplingStrategyBuilder)
                {
                    greedySamplingStrategyBuilder.WithBestOf(configuration.BestOf);
                }
                processorBuilder = greedyBuilder.ParentBuilder;
            }

            speechTranscriptorFactory = new WhisperSpeechTranscriptorFactory(processorBuilder);

            realTimeFactory = new EchoSharpRealtimeTranscriptorFactory(speechTranscriptorFactory, vadDetectorFactory, echoSharpOptions: new EchoSharpRealtimeOptions()
            {
                ConcatenateSegmentsToPrompt = false
            });

            realTimeTranscriptor = realTimeFactory.Create(new RealtimeSpeechTranscriptorOptions()
            {
                AutodetectLanguageOnce = configuration.AutodetectLanguageOnce,
                IncludeSpeechRecogizingEvents = configuration.IncludeRecognizingEvents,
                RetrieveTokenDetails = configuration.RetrieveTokenDetails,
                LanguageAutoDetect = configuration.UseLanguageDetection,
                Language = configuration.LanguageCulture,
                Prompt = configuration.Prompt
            });

            currentConfiguration = configuration;
            statusLabel.Text = "Status: Ready";
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            startButton.Enabled = false;
            stopButton.Enabled = false;

            try
            {
                await EnsureTranscriptionConfiguredAsync();

                if (waveloop != null)
                {
                    stopButton.Enabled = true;
                    return;
                }

                token?.Dispose();
                token = new CancellationTokenSource();

                waveloop = new WaspiLoopbackAudioSource();
                transcriptTextBox.Clear();

                stopButton.Enabled = true;

                async Task ShowTranscriptAsync()
                {
                    try
                    {
                        await foreach (var transcription in realTimeTranscriptor!.TranscribeAsync(waveloop, token.Token))
                        {
                            switch (transcription)
                            {
                                case RealtimeSessionStarted sessionStarted:
                                    Invoke(() => statusLabel.Text = $"Status: Session {sessionStarted.SessionId} started");
                                    break;
                                case RealtimeSessionStopped sessionStopped:
                                    Invoke(() => statusLabel.Text = $"Status: Session {sessionStopped.SessionId} stopped");
                                    break;
                                case RealtimeSegmentRecognizing segmentRecognizing:
                                    Invoke(() =>
                                    {
                                        if (partialResultsCheckBox.Checked)
                                        {
                                            statusLabel.Text = $"Status: Recognizing… {segmentRecognizing.Segment.Text}";
                                        }
                                    });
                                    break;
                                case RealtimeSegmentRecognized segmentRecognized:
                                    Invoke(() =>
                                    {
                                        statusLabel.Text = "Status: Segment recognized";
                                        transcriptTextBox.AppendText($"[{segmentRecognized.Segment.StartTime:hh\\:mm\\:ss}-{(segmentRecognized.Segment.StartTime + segmentRecognized.Segment.Duration):hh\\:mm\\:ss}] {segmentRecognized.Segment.Text}{Environment.NewLine}");
                                        transcriptTextBox.SelectionStart = transcriptTextBox.TextLength;
                                        transcriptTextBox.ScrollToCaret();
                                    });
                                    break;
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        Invoke(() => statusLabel.Text = "Status: Cancelled");
                    }
                    finally
                    {
                        Invoke(() => stopButton.Enabled = false);
                        Invoke(() => startButton.Enabled = true);
                    }
                }

                waveloop.StartRecording();
                statusLabel.Text = "Status: Recording";
                showTranscriptTask = ShowTranscriptAsync();
            }
            catch (Exception ex)
            {
                DisposeFactories();
                statusLabel.Text = $"Status: Error - {ex.Message}";
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StopTranscription();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            StopTranscription();
            DisposeFactories();
            token?.Dispose();
            token = null;
        }

        private WhisperConfiguration BuildConfigurationFromUi()
        {
            if (modelComboBox.SelectedItem is not ModelOption modelOption)
            {
                throw new InvalidOperationException("Please select a model");
            }

            var languageCode = languageTextBox.Text.Trim();
            CultureInfo culture;
            try
            {
                culture = string.IsNullOrWhiteSpace(languageCode)
                    ? CultureInfo.GetCultureInfo("en-US")
                    : CultureInfo.GetCultureInfo(languageCode);
            }
            catch (CultureNotFoundException ex)
            {
                throw new InvalidOperationException("Invalid language code", ex);
            }

            var prompt = string.IsNullOrWhiteSpace(promptTextBox.Text) ? null : promptTextBox.Text;

            return new WhisperConfiguration(
                modelOption.ModelType,
                translateCheckBox.Checked,
                autoDetectLanguageCheckBox.Checked,
                culture.Name,
                culture,
                (float)temperatureNumeric.Value,
                (int)beamSizeNumeric.Value,
                (int)bestOfNumeric.Value,
                (float)vadThresholdNumeric.Value,
                (float)vadGapNumeric.Value,
                partialResultsCheckBox.Checked,
                autodetectOnceCheckBox.Checked,
                tokenDetailsCheckBox.Checked,
                (int)threadsNumeric.Value,
                prompt
            );
        }

        private void StopTranscription()
        {
            token?.Cancel();

            if (waveloop != null)
            {
                waveloop.StopRecording();
                waveloop.Dispose();
                waveloop = null;
            }

            token?.Dispose();
            token = null;

            showTranscriptTask = null;

            stopButton.Enabled = false;
            startButton.Enabled = true;
            statusLabel.Text = "Status: Idle";
        }

        private void DisposeFactories()
        {
            speechTranscriptorFactory?.Dispose();
            speechTranscriptorFactory = null;

            factory?.Dispose();
            factory = null;

            vadDetectorFactory = null;
            realTimeFactory = null;
            realTimeTranscriptor = null;
            currentConfiguration = null;
        }

        private sealed record ModelOption(string DisplayName, GgmlType ModelType)
        {
            public override string ToString() => DisplayName;
        }

        private readonly record struct WhisperConfiguration(
            GgmlType ModelType,
            bool Translate,
            bool UseLanguageDetection,
            string LanguageCode,
            CultureInfo LanguageCulture,
            float Temperature,
            int BeamSize,
            int BestOf,
            float VadThreshold,
            float VadThresholdGap,
            bool IncludeRecognizingEvents,
            bool AutodetectLanguageOnce,
            bool RetrieveTokenDetails,
            int Threads,
            string? Prompt);

    }
}
