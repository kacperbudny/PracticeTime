using MetronomeApp.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
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

        private void ChangeTempoButton_Click(object sender, RoutedEventArgs e)
        {
            metronome.SetTempo(1000);
        }
    }
}