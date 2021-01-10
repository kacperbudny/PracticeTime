using MetronomeApp.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MetronomeApp
{
    /// <summary>
    /// Interaction logic for AddExerciseWindow.xaml
    /// </summary>
    public partial class AddExerciseWindow : Window
    {
        List<Exercise> exercises;

        public AddExerciseWindow(List<Exercise> exercises)
        {
            InitializeComponent();

            this.Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            this.exercises = exercises;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text.Trim() == "" || StartingTempoTextBox.Text.Trim() == "" || TargetTempoTextBox.Text.Trim() == "" || PracticeTimeTextBox.Text.Trim() == "")
            {
                ShowErrorMessage("You have to enter data into all fields except for the notes.");
                return;
            }
            if (exercises.Any(ex => ex.Name == NameTextBox.Text.Trim()))
            {
                ShowErrorMessage("Exercise with that name already exists.");
                return;
            }
            if(Int32.Parse(StartingTempoTextBox.Text) > 300 || Int32.Parse(StartingTempoTextBox.Text) < 40 || Int32.Parse(TargetTempoTextBox.Text) > 300 || Int32.Parse(TargetTempoTextBox.Text) < 40)
            {
                ShowErrorMessage("The tempo must be between 40 and 300.");
                return;
            }
            if (Int32.Parse(PracticeTimeTextBox.Text) > 60 || Int32.Parse(PracticeTimeTextBox.Text) < 1)
            {
                ShowErrorMessage("The practice time must be between 1 and 60.");
                return;
            }
            if (Int32.Parse(StartingTempoTextBox.Text) > Int32.Parse(TargetTempoTextBox.Text))
            {
                ShowErrorMessage("Starting tempo cannot be higher than the target tempo.");
                return;
            }

            Exercise exercise = new Exercise()
            {
                Name = NameTextBox.Text.Trim(),
                PracticeTime = Int32.Parse(PracticeTimeTextBox.Text),
                StartingTempo = Int32.Parse(StartingTempoTextBox.Text),
                CurrentTempo = Int32.Parse(StartingTempoTextBox.Text),
                TargetTempo = Int32.Parse(TargetTempoTextBox.Text),
                Notes = NotesTextBox.Text.Trim(),
                IsInSessionMode = false
            };


            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Exercise>();
                connection.Insert(exercise);
            }

            Close();
        }

        private void ShowErrorMessage(string message)
        {
            WarningTextBlock.Text = message;
            WarningTextBlock.Visibility = Visibility.Visible;
            SystemSounds.Asterisk.Play();
        }

        private void PreviewNumericTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Utilities.AllowNumericInputOnly(e);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PreviewSpaceKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = Utilities.BlockSpaceInput(e);
        }
    }
}
