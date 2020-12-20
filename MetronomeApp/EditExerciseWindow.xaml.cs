using MetronomeApp.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for EditExerciseWindow.xaml
    /// </summary>
    public partial class EditExerciseWindow : Window
    {
        List<Exercise> exercises;
        Exercise exercise;

        public EditExerciseWindow(List<Exercise> exercises, Exercise exercise)
        {
            InitializeComponent();

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
            if (exercises.Any(ex => ex.Name == NameTextBox.Text.Trim()))
            {
                MessageBox.Show("Exercise with that name already exists. Please choose a different name.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (NameTextBox.Text.Trim() == "" || CurrentTempoTextBox.Text.Trim() == "" || TargetTempoTextBox.Text.Trim() == "" || PracticeTimeTextBox.Text.Trim() == "")
            {
                MessageBox.Show("You have to enter data into all fields except for the notes.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (Int32.Parse(CurrentTempoTextBox.Text) > 300 || Int32.Parse(CurrentTempoTextBox.Text) < 40 || Int32.Parse(TargetTempoTextBox.Text) > 300 || Int32.Parse(TargetTempoTextBox.Text) < 40)
            {
                MessageBox.Show("The tempo must be between 40 and 300.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (Int32.Parse(PracticeTimeTextBox.Text) > 60 || Int32.Parse(PracticeTimeTextBox.Text) < 1)
            {
                MessageBox.Show("The practice time must be between 1 and 60.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (Int32.Parse(CurrentTempoTextBox.Text) > Int32.Parse(TargetTempoTextBox.Text))
            {
                MessageBox.Show("Current tempo cannot be higher than the target tempo.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            exercise.Name = NameTextBox.Text.Trim();
            exercise.PracticeTime = Int32.Parse(PracticeTimeTextBox.Text);
            exercise.StartingTempo = Int32.Parse(CurrentTempoTextBox.Text);
            exercise.CurrentTempo = Int32.Parse(CurrentTempoTextBox.Text);
            exercise.TargetTempo = Int32.Parse(TargetTempoTextBox.Text);
            exercise.Notes = NotesTextBox.Text.Trim();

            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Exercise>();
                connection.Update(exercise);
            }

            Close();
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
