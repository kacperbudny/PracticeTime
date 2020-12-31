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
        public ObservableCollection<Tuple<string, string>> MyCollection { get; set; }

        public ShortcutsListWindow()
        {
            InitializeComponent();

            MyCollection = new ObservableCollection<Tuple<string, string>>();

            List<RoutedUICommand> commands = new List<RoutedUICommand>();

            var fieldValues = typeof(KeyboardCommands).GetFields();

            foreach (var field in fieldValues)
            {
                object com = field.GetValue(typeof(RoutedUICommand));
                commands.Add((RoutedUICommand)com);
            }

            List<KeyGesture> inputGestures = new List<KeyGesture>();

            foreach (var com in commands)
            {
                InputGesture[] inputs = new InputGesture[1];
                com.InputGestures.CopyTo(inputs, 0);
                inputGestures.Add((KeyGesture)inputs[0]);
            }

            List<string> commandStrings = new List<string>();

            foreach (var input in inputGestures)
            {
                commandStrings.Add(input.DisplayString);
            }

            List<Tuple<string, string>> finalCommands = new List<Tuple<string, string>>();

            for(int i=0;i<commands.Count();i++)
            {
                var temp = Tuple.Create(commands[i].Text, commandStrings[i]);
                MyCollection.Add(temp);
            }
            
            ShortcutsListView.ItemsSource = MyCollection;
        }
    }
}
