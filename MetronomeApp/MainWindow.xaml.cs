using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MetronomeApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SoundPlayer simpleSound = new SoundPlayer(@"C:\Users\kapib\source\repos\MetronomeApp\MetronomeApp\Resources\metronome.wav");
        bool isMetronomeOn = false;
        //System.Timers.Timer timer = new System.Timers.Timer();
        List<string> czasy;
        RealTimeTimerTest metronome = new RealTimeTimerTest();


        public MainWindow()
        {
            InitializeComponent();
            //timer.Elapsed += Timer_Elapsed;
            //timer.Interval += 500;
            czasy = new List<string>();
        }

        //private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    simpleSound.Play();
        //    czasy.Add(DateTime.Now.ToString("ss.ffff"));
        //}

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (isMetronomeOn == false)
            {
                StartButton.Content = "STOP";
                isMetronomeOn = true;
                //await StartMetronomeAsync();
                //timer.Start();
                await metronome.Run();

            }
            else
            {
                isMetronomeOn = false;
                StartButton.Content = "START";
                //timer.Stop();
                metronome.IsMetronomePlaying = false;

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\kapib\Desktop\\czasy async bez dzwieku.txt"))
                {
                    foreach (string czas in czasy)
                    {

                        file.WriteLine(czas);

                    }
                }
            }
        }

        private async Task StartMetronomeAsync()
        {
            while (isMetronomeOn)
            {
                simpleSound.Play();
                czasy.Add(DateTime.Now.ToString("ss.ffff"));
                await Task.Delay(500);
            }
        }
    }
}

public class RealTimeTimerTest
{
    List<DateTime> lst = new List<DateTime>();
    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
    SoundPlayer simpleSound = new SoundPlayer(@"C:\Users\kapib\source\repos\MetronomeApp\MetronomeApp\Resources\metronome.wav");
    public bool IsMetronomePlaying { get; set; }

    public async Task Run()
    {
        int Tick = 500;
        int Sleep = Tick - 20;
        long OldElapsedMilliseconds = 0;
        IsMetronomePlaying = true;
        sw.Start();

        while (sw.IsRunning && IsMetronomePlaying)
        {
            long ElapsedMilliseconds = sw.ElapsedMilliseconds;
            long mod = (ElapsedMilliseconds % Tick);

            if (OldElapsedMilliseconds != ElapsedMilliseconds && (mod == 0 || ElapsedMilliseconds > Tick))
            {

                //-----------------Do here whatever you want to do--------------Start
                simpleSound.Play();
                lst.Add(DateTime.Now);

                //-----------------Do here whatever you want to do--------------End

                //-----------------Restart----------------Start
                OldElapsedMilliseconds = ElapsedMilliseconds;
                OldElapsedMilliseconds = 0;
                sw.Reset();
                sw.Start();

                await Task.Delay(Sleep);
                //-----------------Restart----------------End
            }

            //------------Must define some condition to break the loop here-----------Start

            //if (lst.Count > 10)
            //{
            //    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\kapib\Desktop\\czasy async bez dzwieku.txt"))
            //    {
            //        foreach (DateTime czas in lst)
            //        {

            //            file.WriteLine(czas.ToString("ss.ffff"));

            //        }
            //    }
            //    break;
            //}
            //-------------Must define some condition to break the loop here-----------End
        }
    }
}