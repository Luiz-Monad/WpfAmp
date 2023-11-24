using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace WpfVisualizer
{
    internal class Source
    {
        private WasapiLoopbackCapture capture;
        private BufferedWaveProvider bufferedWaveProvider = new BufferedWaveProvider(WaveFormat.CreateIeeeFloatWaveFormat(44000, 2));

        public void Start()
        {
            if (capture != null)
            {
                Stop();
                return;
            }
            capture = new WasapiLoopbackCapture();
            capture.DataAvailable += OnDataAvailable;
        }

        private void OnDataAvailable(object? sender, WaveInEventArgs waveInEventArgs)
        {
            bufferedWaveProvider.AddSamples(waveInEventArgs.Buffer, 0, waveInEventArgs.BytesRecorded);
        }

        public bool Running { get => capture?.CaptureState == CaptureState.Capturing; }

        public void Stop()
        {
            if (capture == null) return;
            capture.DataAvailable -= OnDataAvailable;
        }

        public ISampleProvider Provider {
            get => new WaveToSampleProvider(bufferedWaveProvider);
        }
    }
}
