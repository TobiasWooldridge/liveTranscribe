using EchoSharp.Audio;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.IO;

namespace liveTranscribe
{
    internal class WaspiLoopbackAudioSource : AwaitableWaveFileSource
    {
        private readonly WasapiLoopbackCapture waveloop;
        private readonly WaveFormat inputFormat;

        public WaspiLoopbackAudioSource()
            : base()
        {
            waveloop = new WasapiLoopbackCapture();
            inputFormat = waveloop.WaveFormat;

            Initialize(new AudioSourceHeader()
            {
                BitsPerSample = 16,
                Channels = 1,
                SampleRate = 16000
            });

            waveloop.DataAvailable += Waveloop_DataAvailable;
            waveloop.RecordingStopped += Waveloop_RecordingStopped;
        }

        private byte[] ToPcm16(byte[] buffer, int length)
        {
            if (length == 0)
            {
                return Array.Empty<byte>();
            }

            using var memStream = new MemoryStream(buffer, 0, length);
            using var inputStream = new RawSourceWaveStream(memStream, inputFormat);

            ISampleProvider sampleProvider = new WaveToSampleProvider(inputStream);

            if (inputFormat.Channels > 1)
            {
                var stereoToMono = new StereoToMonoSampleProvider(sampleProvider)
                {
                    LeftVolume = 0.5f,
                    RightVolume = 0.5f
                };
                sampleProvider = stereoToMono;
            }

            if (sampleProvider.WaveFormat.SampleRate != 16000)
            {
                sampleProvider = new WdlResamplingSampleProvider(sampleProvider, 16000);
            }

            var convertedProvider = new SampleToWaveProvider16(sampleProvider);
            var convertedBuffer = new byte[Math.Max(convertedProvider.WaveFormat.AverageBytesPerSecond / 4, 2048)];

            using var stream = new MemoryStream();
            int read;
            while ((read = convertedProvider.Read(convertedBuffer, 0, convertedBuffer.Length)) > 0)
            {
                stream.Write(convertedBuffer, 0, read);
            }

            return stream.ToArray();
        }

        public void StartRecording()
        {
            waveloop.StartRecording();
        }

        public void StopRecording()
        {
            waveloop.StopRecording();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                waveloop.DataAvailable -= Waveloop_DataAvailable;
                waveloop.RecordingStopped -= Waveloop_RecordingStopped;
                waveloop.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Waveloop_DataAvailable(object? sender, WaveInEventArgs e)
        {
            var buffer = ToPcm16(e.Buffer, e.BytesRecorded);
            if (buffer.Length > 0)
            {
                WriteData(buffer.AsMemory());
            }
        }

        private void Waveloop_RecordingStopped(object? sender, StoppedEventArgs e)
        {
            if (e.Exception != null)
            {
                throw e.Exception;
            }

            Flush();
        }
    }
}
