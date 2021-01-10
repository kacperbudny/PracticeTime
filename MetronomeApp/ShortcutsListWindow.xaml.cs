using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace MetronomeApp.Classes
{
    /// <summary>
    /// Interaction logic for ShortcutsListWindow.xaml
    /// </summary>
    public partial class ShortcutsListWindow : Window
    {
        public ObservableCollection<Tuple<string, string>> KeyboardShortcuts { get; set; }

        public ShortcutsListWindow()
        {
            InitializeComponent();

            Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            KeyboardShortcuts = new ObservableCollection<Tuple<string, string>>();

            System.Reflection.FieldInfo[] fieldValues = typeof(KeyboardCommands).GetFields();

            foreach (System.Reflection.FieldInfo field in fieldValues)
            {
                RoutedUICommand command = (RoutedUICommand)field.GetValue(typeof(RoutedUICommand));
                InputGesture[] inputs = new InputGesture[1];
                command.InputGestures.CopyTo(inputs, 0);
                KeyGesture keyGesture = (KeyGesture)inputs[0];
                KeyboardShortcuts.Add(Tuple.Create(command.Text, keyGesture.DisplayString));
            }

            ShortcutsListView.ItemsSource = KeyboardShortcuts;
        }
    }
}
