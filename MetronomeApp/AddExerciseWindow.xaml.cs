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
            if(exercises.Any(ex => ex.Name == NameTextBox.Text.Trim()))
            {
                MessageBox.Show("Exercise with that name already exists. Please choose a different name.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(NameTextBox.Text.Trim() == "" || StartingTempoTextBox.Text.Trim() == "" || TargetTempoTextBox.Text.Trim() == "" || PracticeTimeTextBox.Text.Trim() == "")
            {
                MessageBox.Show("You have to enter data into all fields except for the notes.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(Int32.Parse(StartingTempoTextBox.Text) > 300 || Int32.Parse(StartingTempoTextBox.Text) < 40 || Int32.Parse(TargetTempoTextBox.Text) > 300 || Int32.Parse(TargetTempoTextBox.Text) < 40)
            {
                MessageBox.Show("The tempo must be between 40 and 300.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (Int32.Parse(PracticeTimeTextBox.Text) > 60 || Int32.Parse(PracticeTimeTextBox.Text) < 1)
            {
                MessageBox.Show("The practice time must be between 1 and 60.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (Int32.Parse(StartingTempoTextBox.Text) > Int32.Parse(TargetTempoTextBox.Text))
            {
                MessageBox.Show("Starting tempo cannot be higher than the target tempo.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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
