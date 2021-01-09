using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MetronomeApp.Properties;
using NAudio.Wave;

namespace MetronomeApp.Classes
{
    public class Metronome
    {
        public bool IsEnabled { get; set; }
        public int SleepTime { get; private set; }

        readonly Stopwatch stopwatch = new Stopwatch();
        readonly AudioPlayer metronomeSound = new AudioPlayer(SoundType.Metronome);
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
                long ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                long mod = (ElapsedMilliseconds % tickTime);

                if (ElapsedMilliseconds != 0 && (mod == 0 || ElapsedMilliseconds > tickTime))
                {
                    metronomeSound.Play();
                    stopwatch.Restart();

                    if (!IsEnabled) break;

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
