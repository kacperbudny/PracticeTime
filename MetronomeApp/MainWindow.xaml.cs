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

namespace MetronomeApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Metronome metronome = new Metronome();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (metronome.IsMetronomePlaying == false)
            {
                StartButton.Content = "STOP";
                metronome.IsMetronomePlaying = true;
                await metronome.Run();

            }
            else
            {
                metronome.IsMetronomePlaying = false;
                metronome.Stop();
                StartButton.Content = "START";
            }
        }

        private void TempoBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public static bool IsValid(string str)
        {
            int i;
            return int.TryParse(str, out i) && i >= 5 && i <= 9999;
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
                else if(tempo > 300)
                {
                    TempoBox.Text = "300";
                    tempo = 300;
                }
                metronome.SetTempo(tempo);
            }
        }

        private void TempoDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(TempoBox.Text, out int tempo))
            {
                tempo--;
                TempoBox.Text = tempo.ToString();
                metronome.SetTempo(tempo);
            }
        }

        private void TempoUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(TempoBox.Text, out int tempo))
            {
                tempo++;
                TempoBox.Text = tempo.ToString();
                metronome.SetTempo(tempo);
            }
        }
    }
}