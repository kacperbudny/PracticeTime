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
        public bool IsMetronomePlaying { get; set; }
        public int Sleep { get; private set; }

        readonly List<DateTime> times = new List<DateTime>();
        readonly Stopwatch sw = new Stopwatch();
        readonly AudioPlayer metronomeSound = new AudioPlayer(SoundType.Metronome);

        private int Tick;

        public Metronome()
        {
            Tick = 500;
            IsMetronomePlaying = false;
        }

        public async Task Run()
        {
            Sleep = Tick - 20;
            IsMetronomePlaying = true;

            sw.Start();

            while (sw.IsRunning && IsMetronomePlaying)
            {
                long ElapsedMilliseconds = sw.ElapsedMilliseconds;
                long mod = (ElapsedMilliseconds % Tick);

                if (ElapsedMilliseconds != 0 && (mod == 0 || ElapsedMilliseconds > Tick))
                {
                    metronomeSound.Play();
                    sw.Restart();

                    if (!IsMetronomePlaying) break;

                    await Task.Delay(Sleep);
                    metronomeSound.Reset();
                }
            }
        }

        public void Stop()
        {
            IsMetronomePlaying = false;
            sw.Reset();
        }

        public void SetTempo(int tempo)
        {
            tempo = 60000 / tempo;
            Tick = tempo;
            Sleep = Tick - 20;
        }

        public async Task SetTempoAndRun(int tempo)
        {
            Stop();
            await Task.Delay(Sleep + 100);
            SetTempo(tempo);
            await Run();
        }

        public void SetVolume(float newVolume)
        {
            metronomeSound.SetVolume(newVolume);
        }
    }
}
