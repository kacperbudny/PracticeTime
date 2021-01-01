using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MetronomeApp.Properties;
using NAudio.Wave;

namespace MetronomeApp.Classes
{
    public class Metronome : IDisposable
    {
        public bool IsMetronomePlaying { get; set; }
        public int Sleep { get; private set; }

        readonly List<DateTime> times = new List<DateTime>();
        readonly Stopwatch sw = new Stopwatch();
        readonly WaveOutEvent outputDevice = new WaveOutEvent();
        readonly AudioFileReader audioFile;

        private int Tick;

        public Metronome()
        {
            Tick = 500;
            IsMetronomePlaying = false;
            string fileName = "metronome.wav";
            string path = Path.Combine(Environment.CurrentDirectory, "Resources", fileName);
            audioFile = new AudioFileReader(path);
            outputDevice.Init(audioFile);
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
                    outputDevice.Play();
                    times.Add(DateTime.Now);

                    sw.Restart();

                    if (!IsMetronomePlaying) break;

                    await Task.Delay(Sleep);
                    outputDevice.Stop();
                    audioFile.Position = 0;
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

        public void Dispose()
        {
            outputDevice.Dispose(); 
            audioFile.Dispose();
        }
    }
}
