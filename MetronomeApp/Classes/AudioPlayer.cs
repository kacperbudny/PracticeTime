using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MetronomeApp.Classes
{
    class AudioPlayer
    {
        readonly WaveOutEvent outputDevice = new WaveOutEvent();
        readonly AudioFileReader audioFile;

        public AudioPlayer(string soundType)
        {
            string fileName = soundType + ".wav";
            string path = Path.Combine(Environment.CurrentDirectory, "Resources", fileName);

            try
            {
                audioFile = new AudioFileReader(path);
            }
            catch
            {
                MessageBox.Show("Couldn't load the file " + fileName, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }

            outputDevice.Init(audioFile);
        }

        public void Play()
        {
            outputDevice.Play();
        }

        public void Reset()
        {
            outputDevice.Stop();
            audioFile.Position = 0;
        }

        public void SetVolume(float newVolume)
        {
            outputDevice.Volume = newVolume / 100f;
        }
    }
}
