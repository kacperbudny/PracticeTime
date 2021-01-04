using MetronomeApp.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MetronomeApp.Classes
{
    public class TrainingSession
    {
        private readonly Stopwatch sessionTimer;
        private readonly AudioPlayer clockHighSound;
        private readonly AudioPlayer clockLowSound;
        private readonly AudioPlayer completedSound;
        public string SessionTime 
        { 
            get
            {
                return sessionTimer.Elapsed.ToString(@"hh\:mm\:ss");
            } 
        }
        public int CurrentSessionExerciseId { get; set; }
        public int TotalBPMIncrease { get; set; }
        public bool IsEnabled { get; private set; }

        public TrainingSession()
        {
            CurrentSessionExerciseId = 0;
            TotalBPMIncrease = 0;
            IsEnabled = false;
            sessionTimer = new Stopwatch();
            clockHighSound = new AudioPlayer(SoundType.ClockHigh);
            clockLowSound = new AudioPlayer(SoundType.ClockLow);
            completedSound = new AudioPlayer(SoundType.Completed);
        }

        public void Stop()
        {
            sessionTimer.Stop();
            IsEnabled = false;
        }

        public void Start()
        {
            IsEnabled = true;
            CurrentSessionExerciseId = 0;
            TotalBPMIncrease = 0;
            sessionTimer.Restart();
        }

        public void PlayClockHighSound()
        {
            clockHighSound.Play();
        }

        public void PlayClockLowSound()
        {
            clockLowSound.Play();
        }

        public void PlayCompletedSound()
        {
            completedSound.Play();
        }

        public void ResetClockHighSound()
        {
            clockHighSound.Reset();
        }

        public void ResetClockLowSound()
        {
            clockLowSound.Reset();
        }

        public void ResetCompletedSound()
        {
            completedSound.Reset();
        }

        public void SetVolume(float newVolume)
        {
            clockHighSound.SetVolume(newVolume);
            clockLowSound.SetVolume(newVolume);
            completedSound.SetVolume(newVolume);
        }
    }
}