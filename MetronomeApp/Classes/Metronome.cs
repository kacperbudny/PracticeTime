using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MetronomeApp.Properties;

namespace MetronomeApp.Classes
{
    public class Metronome
    {
        List<DateTime> times = new List<DateTime>();
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        SoundPlayer metronomeSound = new SoundPlayer(Resources.metronome);

        public bool IsMetronomePlaying { get; set; }
        private int Tick;
        private int Sleep;

        public Metronome()
        {
            Tick = 500;
            IsMetronomePlaying = false;
            metronomeSound.Play();
        }
        
        public async Task Run()
        {
            Sleep = Tick - 20;
            long OldElapsedMilliseconds = 0;
            IsMetronomePlaying = true;

            sw.Start();

            while (sw.IsRunning && IsMetronomePlaying)
            {
                long ElapsedMilliseconds = sw.ElapsedMilliseconds;
                long mod = (ElapsedMilliseconds % Tick);

                if (OldElapsedMilliseconds != ElapsedMilliseconds && (mod == 0 || ElapsedMilliseconds > Tick))
                {
                    metronomeSound.Play();
                    times.Add(DateTime.Now);

                    //Restarting the counter
                    OldElapsedMilliseconds = ElapsedMilliseconds;
                    OldElapsedMilliseconds = 0;
                    sw.Reset();
                    sw.Start();

                    await Task.Delay(Sleep);
                }
            }

            //if (!IsMetronomePlaying)
            //{
            //    SaveTempoList();
            //}
        }

        public void Stop()
        {
            IsMetronomePlaying = false;
        }

        public void SetTempo(int tempo)
        {
            tempo = 60000 / tempo;
            Tick = tempo;
            Sleep = Tick - 20;
        }

        private void SaveTempoList()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\czasy zmiana tempa.txt"))
            {
                foreach (DateTime time in times)
                {
                    file.WriteLine(time.ToString("ss.fff"));
                }
            }
        }
    }
}
