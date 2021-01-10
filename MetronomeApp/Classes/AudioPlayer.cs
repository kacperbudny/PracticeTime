using NAudio.Wave;
using System;
using System.IO;
using System.Windows;

namespace MetronomeApp.Classes
{
    internal class AudioPlayer
    {
        private readonly WaveOutEvent outputDevice = new WaveOutEvent();
        private readonly AudioFileReader audioFile;

        public AudioPlayer(string soundType)
        {
            string fileName = soundType + ".wav";
            string path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Resources", fileName);

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
