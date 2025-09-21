using EchoSharp.Audio;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Buffers;
using System.IO;

namespace liveTranscribe
{
    internal sealed class WaspiLoopbackAudioSource : AwaitableWaveFileSource
    {
        private readonly WasapiLoopbackCapture loopbackCapture;
        private readonly WaveFormat inputFormat;

        public WaspiLoopbackAudioSource()
        {
            loopbackCapture = new WasapiLoopbackCapture();
            inputFormat = loopbackCapture.WaveFormat;

            Initialize(new AudioSourceHeader
            {
                BitsPerSample = 16,
                Channels = 1,
                SampleRate = 16000
            });

            loopbackCapture.DataAvailable += OnDataAvailable;
            loopbackCapture.RecordingStopped += OnRecordingStopped;
        }

        public void StartRecording()
        {
            loopbackCapture.StartRecording();
        }

        public void StopRecording()
        {
            loopbackCapture.StopRecording();
        }

        private byte[] ConvertToPcm16(byte[] buffer, int bytesRecorded)
        {
            if (bytesRecorded <= 0)
            {
                return Array.Empty<byte>();
            }

            using var memoryStream = new MemoryStream(buffer, 0, bytesRecorded, writable: false);
            using var rawStream = new RawSourceWaveStream(memoryStream, inputFormat);

            ISampleProvider sampleProvider = new WaveToSampleProvider(rawStream);
            if (sampleProvider.WaveFormat.Channels > 1)
            {
                sampleProvider = new StereoToMonoSampleProvider(sampleProvider)
                {
                    LeftVolume = 0.5f,
                    RightVolume = 0.5f
                };
            }

            var resampled = new WdlResamplingSampleProvider(sampleProvider, 16000);
            var pcmProvider = new SampleToWaveProvider16(resampled);

            using var outputStream = new MemoryStream();
            var rentedBuffer = ArrayPool<byte>.Shared.Rent(4096);
            try
            {
                int read;
                while ((read = pcmProvider.Read(rentedBuffer, 0, rentedBuffer.Length)) > 0)
                {
                    outputStream.Write(rentedBuffer, 0, read);
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(rentedBuffer);
            }

            return outputStream.ToArray();
        }

        private void OnDataAvailable(object? sender, WaveInEventArgs e)
        {
            var converted = ConvertToPcm16(e.Buffer, e.BytesRecorded);
            if (converted.Length == 0)
            {
                return;
            }

            WriteData(converted.AsMemory());
        }

        private void OnRecordingStopped(object? sender, StoppedEventArgs e)
        {
            if (e.Exception != null)
            {
                throw e.Exception;
            }

            Flush();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                loopbackCapture.DataAvailable -= OnDataAvailable;
                loopbackCapture.RecordingStopped -= OnRecordingStopped;
                loopbackCapture.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
