using System.Diagnostics;
using System.Threading.Tasks;

namespace MetronomeApp.Classes
{
    public class Metronome
    {
        public bool IsEnabled { get; set; }
        public int SleepTime { get; private set; }

        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly AudioPlayer metronomeSound = new AudioPlayer(SoundType.Metronome);
        private int tickTime;

        public Metronome()
        {
            tickTime = 500;
            IsEnabled = false;
        }

        public async Task Start()
        {
            SleepTime = tickTime - 20;
            IsEnabled = true;

            stopwatch.Start();

            while (stopwatch.IsRunning && IsEnabled)
            {
                long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                long mod = elapsedMilliseconds % tickTime;

                if (elapsedMilliseconds != 0 && (mod == 0 || elapsedMilliseconds > tickTime))
                {
                    metronomeSound.Play();
                    stopwatch.Restart();

                    if (!IsEnabled)
                    {
                        break;
                    }

                    await Task.Delay(SleepTime);
                    metronomeSound.Reset();
                }
            }
        }

        public void Stop()
        {
            IsEnabled = false;
            stopwatch.Reset();
        }

        public void SetTempo(int tempo)
        {
            tempo = 60000 / tempo;
            tickTime = tempo;
            SleepTime = tickTime - 20;
        }

        public void SetVolume(float newVolume)
        {
            metronomeSound.SetVolume(newVolume);
        }
    }
}
