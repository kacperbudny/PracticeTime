using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            this.Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            KeyboardShortcuts = new ObservableCollection<Tuple<string, string>>();

            var fieldValues = typeof(KeyboardCommands).GetFields();

            foreach (var field in fieldValues)
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
