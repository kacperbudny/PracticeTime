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
using MetronomeApp.Properties;
using SQLite;
using System.IO;

namespace MetronomeApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Metronome metronome = new Metronome();
        readonly TapTempo tapTempo = new TapTempo();
        readonly TimekeeperHelper timekeeperHelper = new TimekeeperHelper();

        readonly DispatcherTimer timekeeper = new DispatcherTimer();
        readonly Stopwatch sw = new Stopwatch();

        public MainWindow()
        {
            InitializeComponent();

            timekeeper.Tick += new EventHandler(timer_Tick);
            timekeeper.Interval = new TimeSpan(0, 0, 1);

            UpdateTimerLabel();

            if(!Directory.Exists(App.applicationDirectoryPath))
            {
                Directory.CreateDirectory(App.applicationDirectoryPath);
            }

            ReadDatabase();

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

        // CHANGING TEMPO

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

        // TAP TEMPO

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

        // TIMER

        private void timer_Tick(object sender, EventArgs e)
        {
            if (timekeeperHelper.Time > 0)
            {
                timekeeperHelper.Time--;
                UpdateTimerLabel();
            }
            else
            {
                if (metronome.IsMetronomePlaying) StopMetronome();
                timekeeper.Stop();
                timekeeperHelper.Complete();
                UpdateTimerLabel();
                StartTimerButton.Content = "Start timer";
                ResetTimerButton.IsEnabled = false;
            }
        }

        private void UpdateTimerLabel()
        {
            TimerLabel.Content = timekeeperHelper.GetFullTime();
        }

        private void StartTimerButton_Click(object sender, RoutedEventArgs e)
        {
            if(!timekeeper.IsEnabled)
            {
                StartTimerButton.Content = "Stop timer";
                timekeeper.Start();

                timekeeperHelper.SetTimeToReturn();
                ResetTimerButton.IsEnabled = true;
            }
            else
            {
                StartTimerButton.Content = "Start timer";
                timekeeper.Stop();
            }
        }

        private void TimerDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (timekeeperHelper.Time > 60)
            {
                timekeeperHelper.Time -= 60;
                UpdateTimerLabel();
            } 
        }

        private void TimerUpButton_Click(object sender, RoutedEventArgs e)
        {
            timekeeperHelper.Time += 60;
            UpdateTimerLabel();
        }

        private void ResetTimerButton_Click(object sender, RoutedEventArgs e)
        {
            timekeeper.Stop();
            timekeeperHelper.Reset();
            UpdateTimerLabel();
            StartTimerButton.Content = "Start timer";
            ResetTimerButton.IsEnabled = false;
        }

        // EXERCICES

        private void AddExerciseButton_Click(object sender, RoutedEventArgs e)
        {
            Exercise exercise = new Exercise()
            {
                Name = "SCOM",
                PracticeTime = 5,
                StartingTempo = 150,
                TargetTempo = 220,
                Notes = "Tak"
            };

            
            using(SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Exercise>();
                connection.Insert(exercise);
            }
        }

        void ReadDatabase()
        {
            List<Exercise> exercises = new List<Exercise>();

            using(SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<Exercise>();
                exercises = conn.Table<Exercise>().ToList();
            }

            if(exercises != null)
            {
                ExercisesListView.ItemsSource = exercises;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ReadDatabase();
        }

        private void DeleteExerciseButton_Click(object sender, RoutedEventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<Exercise>();
                conn.DeleteAll<Exercise>();
            }
        }

        private void EditExerciseButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ApplyMetronomeSettingsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}