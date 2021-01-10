using MetronomeApp.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Input;

namespace MetronomeApp
{
    /// <summary>
    /// Interaction logic for EditExerciseWindow.xaml
    /// </summary>
    public partial class EditExerciseWindow : Window
    {
        private readonly List<Exercise> exercises;
        private readonly Exercise exercise;

        public EditExerciseWindow(List<Exercise> exercises, Exercise exercise)
        {
            InitializeComponent();

            Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            NameTextBox.Text = exercise.Name;
            PracticeTimeTextBox.Text = exercise.PracticeTime.ToString();
            CurrentTempoTextBox.Text = exercise.CurrentTempo.ToString();
            TargetTempoTextBox.Text = exercise.TargetTempo.ToString();
            NotesTextBox.Text = exercise.Notes;

            this.exercise = exercise;
            this.exercises = exercises;

            exercises.Remove(exercise);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text.Trim() == "" || CurrentTempoTextBox.Text.Trim() == "" || TargetTempoTextBox.Text.Trim() == "" || PracticeTimeTextBox.Text.Trim() == "")
            {
                ShowErrorMessage("You have to enter data into all fields except for the notes.");
                return;
            }
            if (exercises.Any(ex => ex.Name == NameTextBox.Text.Trim()))
            {
                ShowErrorMessage("Exercise with that name already exists.");
                return;
            }
            if (int.Parse(CurrentTempoTextBox.Text) > 300 || int.Parse(CurrentTempoTextBox.Text) < 40 || int.Parse(TargetTempoTextBox.Text) > 300 || int.Parse(TargetTempoTextBox.Text) < 40)
            {
                ShowErrorMessage("The tempo must be between 40 and 300.");
                return;
            }
            if (int.Parse(PracticeTimeTextBox.Text) > 60 || int.Parse(PracticeTimeTextBox.Text) < 1)
            {
                ShowErrorMessage("The practice time must be between 1 and 60.");
                return;
            }
            if (int.Parse(CurrentTempoTextBox.Text) > int.Parse(TargetTempoTextBox.Text))
            {
                ShowErrorMessage("Starting tempo cannot be higher than the target tempo.");
                return;
            }

            exercise.Name = NameTextBox.Text.Trim();
            exercise.PracticeTime = int.Parse(PracticeTimeTextBox.Text);
            exercise.StartingTempo = int.Parse(CurrentTempoTextBox.Text);
            exercise.CurrentTempo = int.Parse(CurrentTempoTextBox.Text);
            exercise.TargetTempo = int.Parse(TargetTempoTextBox.Text);
            exercise.Notes = NotesTextBox.Text.Trim();

            DatabaseUtilities.UpdateExercise(exercise);

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
