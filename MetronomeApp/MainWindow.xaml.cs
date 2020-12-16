using MetronomeApp.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
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
        readonly Metronome metronome = new Metronome();
        readonly Stopwatch sw = new Stopwatch();
        readonly TapTempo tapTempo = new TapTempo();
        readonly DispatcherTimer timer = new DispatcherTimer();

        private int time = 300;

        public MainWindow()
        {
            InitializeComponent();

            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);

            UpdateTimerLabel();

            sw.Start();
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (metronome.IsMetronomePlaying == false)
            {
                if(sw.ElapsedMilliseconds > metronome.Sleep + 100)
                {
                    sw.Reset();
                    StartButton.Content = "STOP";

                    await metronome.Run();
                }
            }
            else
            {
                StopMetronome();
            }
        }

        private void StopMetronome()
        {
            sw.Start();
            metronome.Stop();

            StartButton.Content = "START";
        }

        private void TempoBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private async void TempoBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            async Task<bool> UserKeepsTyping()
            {
                string txt = TempoBox.Text;   // remember text
                await Task.Delay(500);        // wait some
                return txt != TempoBox.Text;  // return that text chaged or not
            }

            if (await UserKeepsTyping()) return;

            // user is done typing, do your stuff 
            if (Int32.TryParse(TempoBox.Text, out int tempo))
            {
                if (tempo < 40)
                {
                    TempoBox.Text = "40";
                    tempo = 40;
                }
                else if (tempo > 300)
                {
                    TempoBox.Text = "300";
                    tempo = 300;
                }

                if (metronome.IsMetronomePlaying)
                {
                    await metronome.SetTempoAndRun(tempo);
                }
                else metronome.SetTempo(tempo);
            }
        }

        private void TempoDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(TempoBox.Text, out int tempo) && tempo>40)
            {
                tempo--;
                TempoBox.Text = tempo.ToString();
            }
        }

        private void TempoUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(TempoBox.Text, out int tempo) && tempo<300)
            {
                tempo++;
                TempoBox.Text = tempo.ToString();
            }
        }

        private void TapTempoButton_Click(object sender, RoutedEventArgs e)
        {
            if(metronome.IsMetronomePlaying == true)
            {
                StopMetronome();
            }

            if(tapTempo.IsTapTempoModeEnabled == false)
            {
                StartButton.IsEnabled = false;
                CancelTapTempoButton.IsEnabled = true;
                TapTempoButton.Content = "TAP HERE";

                tapTempo.Start();
            }
            else
            {
                tapTempo.NextTap();
                TapTempoButton.Content = tapTempo.FinalTempo.ToString();

                if(tapTempo.AreTapTimesNotEmpty())
                {
                    AcceptTapTempoButton.IsEnabled = true;
                }
            }
        }

        private async void AcceptTapTempoButton_Click(object sender, RoutedEventArgs e)
        {
            TempoBox.Text = tapTempo.FinalTempo.ToString();
            ResetAfterTapTempo();

            await Task.Delay(500);
            await metronome.Run();
            StartButton.Content = "STOP";
        }

        private void CancelTapTempoButton_Click(object sender, RoutedEventArgs e)
        {
            ResetAfterTapTempo();
        }

        private void ResetAfterTapTempo()
        {
            StartButton.IsEnabled = true;
            AcceptTapTempoButton.IsEnabled = false;
            CancelTapTempoButton.IsEnabled = false;
            TapTempoButton.Content = "Tap tempo";

            tapTempo.Reset();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (time > 0)
            {
                time--;
                UpdateTimerLabel();
            }
            else
            {
                timer.Stop();
                if (metronome.IsMetronomePlaying) StopMetronome();
                time = 300;
                UpdateTimerLabel();
                StartTimerButton.Content = "Start timer";
                SystemSounds.Beep.Play();
            }
        }

        private void UpdateTimerLabel()
        {
            string minutes = (time/60) >= 10 ? (time/60).ToString() : "0" + (time/60).ToString();
            string seconds = (time % 60) >= 10 ? (time % 60).ToString() : "0" + (time % 60).ToString();

            TimerLabel.Content = string.Format("{0}:{1}", minutes, seconds);
        }

        private void StartTimerButton_Click(object sender, RoutedEventArgs e)
        {
            if(!timer.IsEnabled)
            {
                StartTimerButton.Content = "Stop timer";
                timer.Start();
            }
            else
            {
                StartTimerButton.Content = "Start timer";
                timer.Stop();
            }
        }

        private void TimerDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (time > 60)
            { 
                time -= 60;
                UpdateTimerLabel();
            } 
        }

        private void TimerUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (time < 3540)
            {
                time += 60;
            }
            else
            {
                time = 3600;
            }

            UpdateTimerLabel();
        }
    }
}