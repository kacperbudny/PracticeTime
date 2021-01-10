using MetronomeApp.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Input;

namespace MetronomeApp
{
    /// <summary>
    /// Interaction logic for AddExerciseWindow.xaml
    /// </summary>
    public partial class AddExerciseWindow : Window
    {
        private readonly List<Exercise> exercises;

        public AddExerciseWindow(List<Exercise> exercises)
        {
            InitializeComponent();

            Owner = Application.Current.MainWindow;
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
            if (int.Parse(StartingTempoTextBox.Text) > 300 || int.Parse(StartingTempoTextBox.Text) < 40 || int.Parse(TargetTempoTextBox.Text) > 300 || int.Parse(TargetTempoTextBox.Text) < 40)
            {
                ShowErrorMessage("The tempo must be between 40 and 300.");
                return;
            }
            if (int.Parse(PracticeTimeTextBox.Text) > 60 || int.Parse(PracticeTimeTextBox.Text) < 1)
            {
                ShowErrorMessage("The practice time must be between 1 and 60.");
                return;
            }
            if (int.Parse(StartingTempoTextBox.Text) > int.Parse(TargetTempoTextBox.Text))
            {
                ShowErrorMessage("Starting tempo cannot be higher than the target tempo.");
                return;
            }

            Exercise exercise = new Exercise()
            {
                Name = NameTextBox.Text.Trim(),
                PracticeTime = int.Parse(PracticeTimeTextBox.Text),
                StartingTempo = int.Parse(StartingTempoTextBox.Text),
                CurrentTempo = int.Parse(StartingTempoTextBox.Text),
                TargetTempo = int.Parse(TargetTempoTextBox.Text),
                Notes = NotesTextBox.Text.Trim(),
                IsInSessionMode = false
            };

            DatabaseUtilities.AddExercise(exercise);

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
