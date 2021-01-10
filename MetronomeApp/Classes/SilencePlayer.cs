using NAudio.Wave;

namespace MetronomeApp.Classes
{
    internal static class SilencePlayer
    {
        public static void Play()
        {
            ISampleProvider silence = new SilenceProvider(new WaveFormat()).ToSampleProvider();
            WaveOutEvent wo = new WaveOutEvent();
            wo.Init(silence);
            wo.Play();
        }
    }
}
