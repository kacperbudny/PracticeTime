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
using System.Windows.Automation.Peers;
using NAudio.Wave;

namespace MetronomeApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables

        readonly Metronome metronome = new Metronome();
        readonly TapTempo tapTempo = new TapTempo();
        readonly TimekeeperHelper timekeeperHelper = new TimekeeperHelper();
        readonly TrainingSession session = new TrainingSession();
        List<Exercise> exercises = new List<Exercise>();

        readonly DispatcherTimer timekeeper = new DispatcherTimer();
        readonly Stopwatch sw = new Stopwatch();
        private ShortcutsListWindow shortcutsListWindow;
        int changeTempoFactor = 1;

        #endregion

        #region Constructor

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

            SilencePlayer.Play();
        }

        #endregion

        #region Metronome

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            await ControlMetronome();
        }

        private async Task ControlMetronome()
        {
            if (metronome.IsMetronomePlaying == false)
            {
                await StartMetronome();
            }
            else
            {
                StopMetronome();
            }
        }

        private async Task StartMetronome()
        {
            while(!metronome.IsMetronomePlaying)
            {
                if (sw.ElapsedMilliseconds > metronome.Sleep + 100)
                {
                    sw.Reset();
                    PlayImage.Visibility = Visibility.Collapsed;
                    PauseImage.Visibility = Visibility.Visible;
                    await metronome.Run();
                    return;
                }
                await Task.Delay(100);
            }
        }

        private void StopMetronome()
        {
            sw.Start();
            metronome.Stop();

            PlayImage.Visibility = Visibility.Visible;
            PauseImage.Visibility = Visibility.Collapsed;
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            metronome.SetVolume((float)VolumeSlider.Value);
            timekeeperHelper.SetVolume((float)VolumeSlider.Value);
            session.SetVolume((float)VolumeSlider.Value);
        }
        #endregion

        #region Changing tempo

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
                string txt = TempoBox.Text;
                await Task.Delay(500);
                return txt != TempoBox.Text;
            }

            if (await UserKeepsTyping()) return;

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
            ReduceTempo(changeTempoFactor);
        }

        private void TempoUpButton_Click(object sender, RoutedEventArgs e)
        {
            IncreaseTempo(changeTempoFactor);
        }

        private void ChangeTempoByOneMenuItem_Click(object sender, RoutedEventArgs e)
        {
            changeTempoFactor = 1;
            TempoUpTextBlock.Text = "";
            TempoDownTextBlock.Text = "";
        }

        private void ChangeTempoByFiveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            changeTempoFactor = 5;
            TempoUpTextBlock.Text = "+5";
            TempoDownTextBlock.Text = "-5";
        }

        private void ChangeTempoByTenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            changeTempoFactor = 10;
            TempoUpTextBlock.Text = "+10";
            TempoDownTextBlock.Text = "-10";
        }

        private void ReduceTempo(int factor)
        {
            if (Int32.TryParse(TempoBox.Text, out int tempo) && (tempo - factor) >= 40)
            {
                TempoBox.Text = (tempo - factor).ToString();
            }
        }

        private void IncreaseTempo(int factor)
        {
            if (Int32.TryParse(TempoBox.Text, out int tempo) && (tempo + factor) <= 300)
            {
                TempoBox.Text = (tempo + factor).ToString();
            }
        }
        #endregion

        #region Tap tempo

        private void TapTempoButton_Click(object sender, RoutedEventArgs e)
        {
            ControlTapTempo();
        }

        private void ControlTapTempo()
        {
            if (metronome.IsMetronomePlaying == true)
            {
                StopMetronome();
            }

            if (tapTempo.IsTapTempoModeEnabled == false)
            {
                StartButton.IsEnabled = false;
                TapTempoButton.Content = "TAP HERE";
                tapTempo.Start();
            }
            else
            {
                tapTempo.NextTap();
                TapTempoButton.Content = tapTempo.FinalTempo.ToString();
            }
        }

        private void ResetAfterTapTempo()
        {
            StartButton.IsEnabled = true;
            TapTempoButton.Content = "Tap tempo";
            tapTempo.Reset();
        }
        #endregion

        #region Timer
        private async void timer_Tick(object sender, EventArgs e)
        {
            if (timekeeperHelper.Time > 0)
            {
                timekeeperHelper.Time--;
                UpdateTimerLabel();
            }
            else
            {
                StopMetronomeAndTimer(true);

                if (session.IsEnabled)
                {
                    SaveCurrentTempoAndUpdateBPMCount();
                    session.CurrentSessionExerciseId++;

                    if (session.CurrentSessionExerciseId < ExercisesListView.Items.Count)
                    {
                        await Task.Delay(1000);
                        await ChangeCurrentExerciseInSessionMode();
                    }
                    else
                    {
                        session.ResetCompletedSound();

                        ExitSessionMode();

                        TotalSessionTimeTextblock.Text = session.SessionTime;
                        TotalExerciseNumberTextblock.Text = session.CurrentSessionExerciseId.ToString();
                        BPMIncreaseTextblock.Text = session.TotalBPMIncrease.ToString();

                        OverlayRectangle.Visibility = Visibility.Visible;
                        InformationGrid.Visibility = Visibility.Visible;

                        session.PlayCompletedSound();
                    }
                }
            }
        }

        private void StartTimerButton_Click(object sender, RoutedEventArgs e)
        {
            ControlTimer();
        }

        private void TimerDownButton_Click(object sender, RoutedEventArgs e)
        {
            ReduceTimerTime();
        }

        private void TimerUpButton_Click(object sender, RoutedEventArgs e)
        {
            IncreaseTimerTime();
        }

        private void ResetTimerButton_Click(object sender, RoutedEventArgs e)
        {
            ResetTimer();
        }

        private void ControlTimer()
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

        private void ReduceTimerTime()
        {
            if (timekeeperHelper.Time > 60)
            {
                timekeeperHelper.Time -= 60;
                UpdateTimerLabel();
            }
        }

        private void IncreaseTimerTime()
        {
            timekeeperHelper.Time += 60;
            UpdateTimerLabel();
        }

        private void StartTimer()
        {
            StartTimerButton.Content = "Stop timer";
            timekeeper.Start();

            timekeeperHelper.SetTimeToReturn();
            ResetTimerButton.IsEnabled = true;
        }

        private void UpdateTimerLabel()
        {
            TimerLabel.Content = timekeeperHelper.GetFullTime();
        }

        private void ResetTimer()
        {
            timekeeper.Stop();
            timekeeperHelper.Reset();
            UpdateTimerLabel();

            StartTimerButton.Content = "Start timer";
            ResetTimerButton.IsEnabled = false;
        }
        #endregion

        #region Exercises
        private void AddExerciseButton_Click(object sender, RoutedEventArgs e)
        {
            AddExercise();
        }

        private void EditExerciseButton_Click(object sender, RoutedEventArgs e)
        {
            EditExercise();
        }

        private void DeleteExerciseButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteExercise();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ReadDatabase();
        }

        private void ExercisesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!session.IsEnabled && ExercisesListView.SelectedIndex != -1)
            {
                DependencyObject obj = (DependencyObject)e.OriginalSource;

                while (obj != null && obj != ExercisesListView)
                {
                    if (obj.GetType() == typeof(ListViewItem))
                    {
                        ApplyMetronomeSettings((Exercise)ExercisesListView.SelectedItem);
                        break;
                    }
                    obj = VisualTreeHelper.GetParent(obj);
                }
            }
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
                if (!session.IsEnabled)
                {
                    EditExerciseButton.IsEnabled = true;
                    DeleteExerciseButton.IsEnabled = true;
                    ApplyMetronomeSettingsButton.IsEnabled = true;
                    SaveMetronomeSettingsIntoItemButton.IsEnabled = true;
                }
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (!session.IsEnabled)
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

                DatabaseUtilities.UpdateExercise(exercise);

                if (ExercisesCategoriesComboBox.SelectedIndex == 1)
                {
                    ReadDatabase();
                }
            }
        }

        private void ListDownButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)e.OriginalSource;
            Exercise exercise = (Exercise)button.DataContext;

            SwapWithNext(exercise);
        }

        private void ListUpButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)e.OriginalSource;
            Exercise exercise = (Exercise)button.DataContext;

            SwapWithPrevious(exercise);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filteredList = exercises.Where(ex => ex.Name.ToLower().Contains(SearchBox.Text.ToLower())).ToList();
            ExercisesListView.ItemsSource = filteredList;
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

        private void ApplyMetronomeSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyMetronomeSettings((Exercise)ExercisesListView.SelectedItem);
        }

        private void SaveMetronomeSettingsIntoItemButton_Click(object sender, RoutedEventArgs e)
        {
            SaveTempoIntoItem((Exercise)ExercisesListView.SelectedItem);
            ReadDatabase();
        }

        private void SwapWithPrevious(Exercise exercise)
        {
            if (ExercisesCategoriesComboBox.SelectedIndex == 1)
            {
                int index = ExercisesListView.Items.IndexOf(exercise);

                if (index != 0)
                {
                    Exercise previousExercise = (Exercise)ExercisesListView.Items.GetItemAt(index - 1);
                    DatabaseUtilities.SwapExerciseSessionOrders(exercise, previousExercise);
                    ReadDatabase();
                    ExercisesListView.SelectedIndex = index - 1;
                }
            }
        }

        private void SwapWithNext(Exercise exercise)
        {
            if (ExercisesCategoriesComboBox.SelectedIndex == 1)
            {
                int index = ExercisesListView.Items.IndexOf(exercise);

                if (exercise.SessionModeOrder != exercises.Max(ex => ex.SessionModeOrder))
                {
                    Exercise nextExercise = (Exercise)ExercisesListView.Items.GetItemAt(index + 1);
                    DatabaseUtilities.SwapExerciseSessionOrders(exercise, nextExercise);
                    ReadDatabase();
                    ExercisesListView.SelectedIndex = index + 1;
                }
            }
        }

        private void ReadDatabase()
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

        private void AddExercise()
        {
            OverlayRectangle.Visibility = Visibility.Visible;

            AddExerciseWindow addExerciseWindow = new AddExerciseWindow(exercises);
            addExerciseWindow.ShowDialog();

            OverlayRectangle.Visibility = Visibility.Collapsed;
            ReadDatabase();
        }

        private void EditExercise()
        {
            OverlayRectangle.Visibility = Visibility.Visible;

            EditExerciseWindow editExerciseWindow = new EditExerciseWindow(exercises, (Exercise)ExercisesListView.SelectedItem);
            editExerciseWindow.ShowDialog();

            OverlayRectangle.Visibility = Visibility.Collapsed;
            ReadDatabase();
        }

        private void DeleteExercise()
        {
            Exercise selectedExercise = (Exercise)ExercisesListView.SelectedItem;

            var result = MessageBox.Show("Are you sure you want to delete this exercise?\n\n" + selectedExercise.Name,
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

        private void ApplyMetronomeSettings(Exercise exercise)
        {
            TempoBox.Text = exercise.CurrentTempo.ToString();
            timekeeperHelper.Time = 60 * exercise.PracticeTime;
            UpdateTimerLabel();
        }

        private void SaveTempoIntoItem(Exercise exercise)
        {
            bool matchCurrentAndTargetTempo = false;

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
                        exercise.CurrentTempo = exercise.TargetTempo;
                        matchCurrentAndTargetTempo = true;
                        break;
                }
            }

            if (!matchCurrentAndTargetTempo)
            {
                exercise.CurrentTempo = Int32.Parse(TempoBox.Text);
            }

            DatabaseUtilities.UpdateExercise(exercise);
        }
        #endregion

        #region Training session

        private async void StartSessionModeButton_Click(object sender, RoutedEventArgs e)
        {
            await ControlSessionMode();
        }

        private async void PreviousSessionExerciseButton_Click(object sender, RoutedEventArgs e)
        {
            await GoToPreviousExercise();
        }

        private async void NextSessionExerciseButton_Click(object sender, RoutedEventArgs e)
        {
            await GoToNextExercise();
        }

        private void AcceptCongtratulationsButton_Click(object sender, RoutedEventArgs e)
        {
            OverlayRectangle.Visibility = Visibility.Collapsed;
            InformationGrid.Visibility = Visibility.Collapsed;
        }

        private async Task GoToPreviousExercise()
        {
            if (session.CurrentSessionExerciseId != 0)
            {
                StopMetronomeAndTimer(false);
                SaveCurrentTempoAndUpdateBPMCount();
                session.CurrentSessionExerciseId--;
                await ChangeCurrentExerciseInSessionMode();
            }
        }

        private async Task GoToNextExercise()
        {
            if (session.CurrentSessionExerciseId < ExercisesListView.Items.Count - 1)
            {
                StopMetronomeAndTimer(false);
                SaveCurrentTempoAndUpdateBPMCount();
                session.CurrentSessionExerciseId++;
                await ChangeCurrentExerciseInSessionMode();
            }
        }

        private async Task ControlSessionMode()
        {
            if (exercises.Where(ex => ex.IsInSessionMode == true).ToList().Count == 0)
            {
                MessageBox.Show("To start a session, you must first add at least one exercise into session mode.",
                "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (session.IsEnabled == false)
            {
                await StartSessionMode();
            }
            else
            {
                var result = MessageBox.Show("Are you sure you want to cancel your sesssion?",
                    "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        ExitSessionMode();
                        break;

                    case MessageBoxResult.No:
                        return;
                }
            }
        }

        private async Task StartSessionMode()
        {
            session.Start();
            ExercisesCategoriesComboBox.SelectedIndex = 1;
            AddExerciseButton.IsEnabled = false;
            EditExerciseButton.IsEnabled = false;
            DeleteExerciseButton.IsEnabled = false;
            RefreshButton.IsEnabled = false;
            ApplyMetronomeSettingsButton.IsEnabled = false;
            SaveMetronomeSettingsIntoItemButton.IsEnabled = false;
            ExercisesCategoriesComboBox.IsEnabled = false;
            SearchBox.IsEnabled = false;
            PreviousSessionExerciseButton.IsEnabled = true;
            NextSessionExerciseButton.IsEnabled = true;
            SessionButtonLabel.Text = "CANCEL SESSION";
            ButtonsColumn.Width = 0;
            SessionColumn.Width = 0;
            SessionIndicator.Fill = Brushes.Lime;
            ExercisesListView.ItemContainerStyle = (Style)FindResource("disableListViewSelectionStyle");
            ExercisesListView.SelectedIndex = session.CurrentSessionExerciseId;
            if (timekeeperHelper.IsEnabled) ResetTimer();
            if (tapTempo.IsTapTempoModeEnabled) ResetAfterTapTempo();

            ApplyMetronomeSettings((Exercise)ExercisesListView.Items[session.CurrentSessionExerciseId]);
            await CountdownDuringSessionAsync();

            await StartMetronomeAndTimerAsync();
        }

        private void ExitSessionMode()
        {
            StopMetronome();
            ResetTimer();

            SessionIndicator.Fill = Brushes.DarkSlateGray;
            AddExerciseButton.IsEnabled = true;
            RefreshButton.IsEnabled = true;
            ExercisesCategoriesComboBox.IsEnabled = true;
            SearchBox.IsEnabled = true;
            PreviousSessionExerciseButton.IsEnabled = false;
            NextSessionExerciseButton.IsEnabled = false;
            SessionButtonLabel.Text = "START SESSION";
            ButtonsColumn.Width = double.NaN;
            SessionColumn.Width = double.NaN;
            ExercisesListView.ItemContainerStyle = (Style)FindResource("enableListViewSelectionStyle");
            session.Stop();
            ReadDatabase();
        }

        private void StopMetronomeAndTimer(bool isCompleteSoundNeeded)
        {
            if (metronome.IsMetronomePlaying)
            {
                StopMetronome();
            }

            timekeeper.Stop();

            if (isCompleteSoundNeeded)
            {
                timekeeperHelper.Complete();
            }
            else
            {
                timekeeperHelper.Reset();
            }

            UpdateTimerLabel();
            StartTimerButton.Content = "Start timer";
            ResetTimerButton.IsEnabled = false;
        }

        private async Task ChangeCurrentExerciseInSessionMode()
        {
            ApplyMetronomeSettings((Exercise)ExercisesListView.Items[session.CurrentSessionExerciseId]);
            ExercisesListView.SelectedIndex = session.CurrentSessionExerciseId;
            await CountdownDuringSessionAsync();
            await StartMetronomeAndTimerAsync();
        }

        private void SaveCurrentTempoAndUpdateBPMCount()
        {
            Exercise exercise = (Exercise)ExercisesListView.Items[session.CurrentSessionExerciseId];
            if(Int32.TryParse(TempoBox.Text, out int tempoFromTempoBox))
            {
                if (exercise.CurrentTempo != tempoFromTempoBox)
                {
                    int currentTempoBeforeUpdating = exercise.CurrentTempo;
                    int targetTempoBeforeUpdating = exercise.TargetTempo;
                    SaveTempoIntoItem(exercise);

                    if (tempoFromTempoBox > exercise.TargetTempo && targetTempoBeforeUpdating == exercise.TargetTempo)
                    {
                        session.TotalBPMIncrease += exercise.TargetTempo - currentTempoBeforeUpdating;
                    }
                    else
                    {
                        session.TotalBPMIncrease += tempoFromTempoBox - currentTempoBeforeUpdating;
                    }

                    ReadDatabase();
                }
            }
        }

        private async Task CountdownDuringSessionAsync()
        {
            session.ResetClockHighSound();

            OverlayRectangle.Visibility = Visibility.Visible;
            CountdownGrid.Visibility = Visibility.Visible;

            ComingSongTextblock.Text = ((Exercise)ExercisesListView.SelectedItem).Name;

            for (int i = 3; i > 0; i--)
            {
                CountdownTextblock.Text = i.ToString();
                session.PlayClockLowSound();
                await Task.Delay(1000);
                session.ResetClockLowSound();
            }

            OverlayRectangle.Visibility = Visibility.Collapsed;
            CountdownGrid.Visibility = Visibility.Collapsed;

            session.PlayClockHighSound();
        }

        private async Task StartMetronomeAndTimerAsync()
        {
            StartTimer();
            await StartMetronome();
        }
        #endregion

        #region Keyboard shortcuts

        private void DisplayShortcutsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (shortcutsListWindow == null)
            {
                shortcutsListWindow = new ShortcutsListWindow();
                shortcutsListWindow.Closed += (a, b) => shortcutsListWindow = null;
                shortcutsListWindow.Show();
            }
            else
            {
                shortcutsListWindow.Show();
            }
        }

        private void StartButton_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !CountdownGrid.IsVisible && !tapTempo.IsTapTempoModeEnabled;
        }

        private void IfCountdownDisabled_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !CountdownGrid.IsVisible;
        }

        private void TapTempoAccept_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tapTempo.AreTapTimesNotEmpty();
        }

        private void TapTempoCancel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tapTempo.IsTapTempoModeEnabled;
        }

        private void ResetTimer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = timekeeperHelper.IsEnabled;
        }

        private void IfNotInSession_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !CountdownGrid.IsVisible && !session.IsEnabled;
        }

        private void ExercisesActions_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !CountdownGrid.IsVisible && !session.IsEnabled && ExercisesListView.SelectedIndex != -1;
        }

        private void GoToListview_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !CountdownGrid.IsVisible && !session.IsEnabled && ExercisesListView.Items.Count != 0;
        }

        private void GoToSessionExercise_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !CountdownGrid.IsVisible && session.IsEnabled;
        }

        private void SwappingExercises_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !CountdownGrid.IsVisible && !session.IsEnabled && ExercisesCategoriesComboBox.SelectedIndex == 1 && ExercisesListView.SelectedIndex != -1;
        }

        private void AcceptCongratulations_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = InformationGrid.IsVisible;
        }

        private async void ControlMetronomeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            await ControlMetronome();
        }

        private void Plus1Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncreaseTempo(1);
        }

        private void Plus5Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncreaseTempo(5);
        }

        private void Plus10Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncreaseTempo(10);
        }

        private void Minus1Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReduceTempo(1);
        }

        private void Minus5Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReduceTempo(5);
        }

        private void Minus10Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReduceTempo(10);
        }

        private void TapTempoStart_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ControlTapTempo();
        }

        private void TapTempoCancel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ResetAfterTapTempo();
        }

        private async void TapTempoAccept_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TempoBox.Text = tapTempo.FinalTempo.ToString();
            ResetAfterTapTempo();
            await StartMetronome();
        }

        private void PlusTimer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncreaseTimerTime();
        }

        private void MinusTimer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReduceTimerTime();
        }

        private void ControlTimer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ControlTimer();
        }

        private void ResetTimer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ResetTimer();
        }

        private void AddExercise_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddExercise();
        }

        private void EditExercise_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            EditExercise();
        }

        private void DeleteExercise_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DeleteExercise();
        }

        private void RefreshList_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReadDatabase();
        }

        private void ChangeView_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (ExercisesCategoriesComboBox.SelectedIndex == 0) 
                ExercisesCategoriesComboBox.SelectedIndex = 1;
            else 
                ExercisesCategoriesComboBox.SelectedIndex = 0;
        }

        private void GoToListview_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (ExercisesListView.SelectedIndex == -1)
                ExercisesListView.SelectedIndex = 0;
        }

        private void AddToSession_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exercise exercise = (Exercise)ExercisesListView.SelectedItem;

            if (exercise.IsInSessionMode == true)
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

            DatabaseUtilities.UpdateExercise(exercise);

            int selectedId = ExercisesListView.SelectedIndex;
            ReadDatabase();
            ExercisesListView.SelectedIndex = selectedId;
        }

        private async void ControlSession_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            await ControlSessionMode();
        }

        private async void PreviousExercise_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            await GoToPreviousExercise();
        }

        private async void NextExercise_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            await GoToNextExercise();
        }

        private void SetUpMetronomeForSelected_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveTempoIntoItem((Exercise)ExercisesListView.SelectedItem);
            ReadDatabase();
        }

        private void SaveMetronomesTempo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ApplyMetronomeSettings((Exercise)ExercisesListView.SelectedItem);
        }

        private void GoToSearch_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SearchBox.Focus();
        }

        private void SwapWithPrevious_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SwapWithPrevious((Exercise)ExercisesListView.SelectedItem);
        }

        private void SwapWithNext_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SwapWithNext((Exercise)ExercisesListView.SelectedItem);
        }

        private void Unfocus_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(SearchBox.IsFocused)
            {
                Keyboard.ClearFocus();
            }
        }

        private void AcceptCongratulations_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OverlayRectangle.Visibility = Visibility.Collapsed;
            InformationGrid.Visibility = Visibility.Collapsed;
        }

        #endregion
    }
}