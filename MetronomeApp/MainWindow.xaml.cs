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
using System.Data;

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
        List<Exercise> exercises = new List<Exercise>();

        readonly DispatcherTimer timekeeper = new DispatcherTimer();
        readonly Stopwatch sw = new Stopwatch();

        bool isSessionModeEnabled = false;
        int currentSessionExercise;

        public MainWindow()
        {
            InitializeComponent();

            timekeeper.Tick += new EventHandler(timer_Tick);
            timekeeper.Interval = new TimeSpan(0, 0, 1);

            UpdateTimerLabel();

            if (!Directory.Exists(App.applicationDirectoryPath))
            {
                Directory.CreateDirectory(App.applicationDirectoryPath);
            }

            ExercisesCategoriesComboBox.SelectedIndex = 0;

            sw.Start();
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (metronome.IsMetronomePlaying == false)
            {
                if (sw.ElapsedMilliseconds > metronome.Sleep + 100)
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
            e.Handled = Utilities.AllowNumericInputOnly(e);
        }

        private void TempoBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = Utilities.BlockSpaceInput(e);
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
            if (Int32.TryParse(TempoBox.Text, out int tempo) && tempo > 40)
            {
                tempo--;
                TempoBox.Text = tempo.ToString();
            }
        }

        private void TempoUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(TempoBox.Text, out int tempo) && tempo < 300)
            {
                tempo++;
                TempoBox.Text = tempo.ToString();
            }
        }

        // TAP TEMPO

        private void TapTempoButton_Click(object sender, RoutedEventArgs e)
        {
            if (metronome.IsMetronomePlaying == true)
            {
                StopMetronome();
            }

            if (tapTempo.IsTapTempoModeEnabled == false)
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

                if (tapTempo.AreTapTimesNotEmpty())
                {
                    AcceptTapTempoButton.IsEnabled = true;
                }
            }
        }

        private async void AcceptTapTempoButton_Click(object sender, RoutedEventArgs e)
        {
            TempoBox.Text = tapTempo.FinalTempo.ToString();
            ResetAfterTapTempo();
            StartButton.Content = "STOP";

            await Task.Delay(500);
            await metronome.Run();
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

        private async void timer_Tick(object sender, EventArgs e)
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

                if (isSessionModeEnabled)
                {
                    Exercise exercise = (Exercise)ExercisesListView.Items[currentSessionExercise];

                    exercise.CurrentTempo = Int32.Parse(TempoBox.Text);

                    using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
                    {
                        connection.CreateTable<Exercise>();
                        connection.Update(exercise);
                    }

                    currentSessionExercise++;

                    ReadDatabase();

                    if (currentSessionExercise < ExercisesListView.Items.Count)
                    {
                        ApplyMetronomeSettings((Exercise)ExercisesListView.Items[currentSessionExercise]);

                        await StartMetronomeAndTimerAsync();
                    }
                    else
                    {
                        MessageBox.Show("Congratulations! You've finished your session.");

                        ExitSessionMode();
                    }
                }
            }
        }

        private void UpdateTimerLabel()
        {
            TimerLabel.Content = timekeeperHelper.GetFullTime();
        }

        private void StartTimerButton_Click(object sender, RoutedEventArgs e)
        {
            if (!timekeeper.IsEnabled)
            {
                StartTimer();
            }
            else
            {
                StartTimerButton.Content = "Start timer";
                timekeeper.Stop();
            }
        }

        private void StartTimer()
        {
            StartTimerButton.Content = "Stop timer";
            timekeeper.Start();

            timekeeperHelper.SetTimeToReturn();
            ResetTimerButton.IsEnabled = true;
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
            OverlayRectangle.Visibility = Visibility.Visible;
            AddExerciseWindow addExerciseWindow = new AddExerciseWindow(exercises);
            addExerciseWindow.ShowDialog();
            OverlayRectangle.Visibility = Visibility.Hidden;
            ReadDatabase();
        }

        void ReadDatabase()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<Exercise>();
                exercises = conn.Table<Exercise>().ToList();
            }

            if (exercises != null)
            {
                if (ExercisesCategoriesComboBox.SelectedIndex == 0)
                {
                    ExercisesListView.ItemsSource = exercises;
                }
                else if (ExercisesCategoriesComboBox.SelectedIndex == 1)
                {
                    ExercisesListView.ItemsSource = exercises.Where(ex => ex.IsInSessionMode == true).ToList().OrderBy(ex => ex.SessionModeOrder);
                }
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ReadDatabase();
        }

        private void DeleteExerciseButton_Click(object sender, RoutedEventArgs e)
        {
            Exercise selectedExercise = (Exercise)ExercisesListView.SelectedItem;

            var result = MessageBox.Show("Are you sure you want to delete this exercise? " + selectedExercise.Name,
                "Deleting exercise", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                    {
                        conn.CreateTable<Exercise>();
                        conn.Delete((Exercise)ExercisesListView.SelectedItem);
                    }
                    ReadDatabase();
                    break;

                case MessageBoxResult.No:
                    break;
            }
        }

        private void EditExerciseButton_Click(object sender, RoutedEventArgs e)
        {
            OverlayRectangle.Visibility = Visibility.Visible;
            EditExerciseWindow editExerciseWindow = new EditExerciseWindow(exercises, (Exercise)ExercisesListView.SelectedItem);
            editExerciseWindow.ShowDialog();
            OverlayRectangle.Visibility = Visibility.Hidden;
            ReadDatabase();
        }

        private void ApplyMetronomeSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Exercise selectedExercise = (Exercise)ExercisesListView.SelectedItem;

            ApplyMetronomeSettings(selectedExercise);
        }

        private void ExercisesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!isSessionModeEnabled)
            {
                DependencyObject obj = (DependencyObject)e.OriginalSource;

                while (obj != null && obj != ExercisesListView)
                {
                    if (obj.GetType() == typeof(ListViewItem))
                    {
                        Exercise selectedExercise = (Exercise)ExercisesListView.SelectedItem;

                        ApplyMetronomeSettings(selectedExercise);

                        break;
                    }
                    obj = VisualTreeHelper.GetParent(obj);
                }
            }
        }

        private void ApplyMetronomeSettings(Exercise exercise)
        {
            TempoBox.Text = exercise.CurrentTempo.ToString();
            timekeeperHelper.Time = 60 * exercise.PracticeTime;
            UpdateTimerLabel();

        }

        private void ExercisesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ExercisesListView.SelectedIndex == -1)
            {
                EditExerciseButton.IsEnabled = false;
                DeleteExerciseButton.IsEnabled = false;
                ApplyMetronomeSettingsButton.IsEnabled = false;
                SaveMetronomeSettingsIntoItemButton.IsEnabled = false;
            }
            else
            {
                if (!isSessionModeEnabled)
                {
                    EditExerciseButton.IsEnabled = true;
                    DeleteExerciseButton.IsEnabled = true;
                    ApplyMetronomeSettingsButton.IsEnabled = true;
                    SaveMetronomeSettingsIntoItemButton.IsEnabled = true;
                }
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filteredList = exercises.Where(ex => ex.Name.ToLower().Contains(SearchBox.Text.ToLower())).ToList();

            ExercisesListView.ItemsSource = filteredList;
        }

        private void SaveMetronomeSettingsIntoItemButton_Click(object sender, RoutedEventArgs e)
        {
            Exercise exercise = (Exercise)ExercisesListView.SelectedItem;

            if (Int32.Parse(TempoBox.Text) > exercise.TargetTempo)
            {
                var result = MessageBox.Show("The current tempo of the metronome is higher than the selected exercise's target tempo.\n\n" +
                    "Do you want to update the target tempo of the selected exercise to match the metronome's tempo?",
                    "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        exercise.TargetTempo = Int32.Parse(TempoBox.Text);
                        break;

                    case MessageBoxResult.No:
                        return;
                }
            }

            exercise.CurrentTempo = Int32.Parse(TempoBox.Text);

            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Exercise>();
                connection.Update(exercise);
            }

            ReadDatabase();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkbox = (CheckBox)e.OriginalSource;
            Exercise exercise = (Exercise)checkbox.DataContext;

            if (checkbox.IsChecked == false)
            {
                exercise.IsInSessionMode = false;
                exercise.SessionModeOrder = 0;
            }
            else
            {
                exercise.IsInSessionMode = true;
                int highestSessionOrder = exercises.Max(ex => ex.SessionModeOrder);
                exercise.SessionModeOrder = highestSessionOrder + 1;
            }

            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Exercise>();
                connection.Update(exercise);
            }

            if (ExercisesCategoriesComboBox.SelectedIndex == 1)
            {
                ReadDatabase();
            }
        }

        private void ExercisesCategoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReadDatabase();
            if (ExercisesCategoriesComboBox.SelectedIndex == 1)
            {
                ButtonsColumn.Width = Double.NaN;
            }
            else
            {
                ButtonsColumn.Width = 0;
            }
        }

        private void ListDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExercisesCategoriesComboBox.SelectedIndex == 1)
            {
                var button = (Button)e.OriginalSource;
                Exercise exercise = (Exercise)button.DataContext;

                if (exercise.SessionModeOrder != exercises.Max(ex => ex.SessionModeOrder))
                {
                    int index = ExercisesListView.Items.IndexOf(exercise);
                    Exercise nextExercise = (Exercise)ExercisesListView.Items.GetItemAt(index + 1);

                    int exerciseSessionOrder = exercise.SessionModeOrder;
                    exercise.SessionModeOrder = nextExercise.SessionModeOrder;
                    nextExercise.SessionModeOrder = exerciseSessionOrder;

                    using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
                    {
                        connection.CreateTable<Exercise>();
                        connection.Update(exercise);
                        connection.Update(nextExercise);
                    }

                    ReadDatabase();
                }
            }
        }

        private void ListUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExercisesCategoriesComboBox.SelectedIndex == 1)
            {
                var button = (Button)e.OriginalSource;
                Exercise exercise = (Exercise)button.DataContext;

                int index = ExercisesListView.Items.IndexOf(exercise);

                if (index != 0)
                {
                    Exercise previousExercise = (Exercise)ExercisesListView.Items.GetItemAt(index - 1);

                    int exerciseSessionOrder = exercise.SessionModeOrder;
                    exercise.SessionModeOrder = previousExercise.SessionModeOrder;
                    previousExercise.SessionModeOrder = exerciseSessionOrder;

                    using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
                    {
                        connection.CreateTable<Exercise>();
                        connection.Update(exercise);
                        connection.Update(previousExercise);
                    }

                    ReadDatabase();
                }
            }
        }

        private async void StartSessionModeButton_Click(object sender, RoutedEventArgs e)
        {
            if(exercises.Where(ex => ex.IsInSessionMode == true).ToList().Count == 0)
            {
                MessageBox.Show("To start a session, you must first add at least one exercise into session mode.",
                "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (isSessionModeEnabled == false)
            {
                isSessionModeEnabled = true;
                ExercisesCategoriesComboBox.SelectedIndex = 1;
                AddExerciseButton.IsEnabled = false;
                EditExerciseButton.IsEnabled = false;
                DeleteExerciseButton.IsEnabled = false;
                RefreshButton.IsEnabled = false;
                ApplyMetronomeSettingsButton.IsEnabled = false;
                SaveMetronomeSettingsIntoItemButton.IsEnabled = false;
                ExercisesCategoriesComboBox.IsEnabled = false;
                SearchBox.IsEnabled = false;
                StartSessionModeButton.Content = "Cancel session";
                ButtonsColumn.Width = 0;
                currentSessionExercise = 0;

                ApplyMetronomeSettings((Exercise)ExercisesListView.Items[currentSessionExercise]);

                await StartMetronomeAndTimerAsync();
            }
            else
            {
                ExitSessionMode();
            }
        }

        private async Task StartMetronomeAndTimerAsync()
        {
            StartButton.Content = "STOP";

            StartTimer();

            await Task.Delay(500);
            await metronome.Run();
        }

        private void ExitSessionMode()
        {

        }
    }
}