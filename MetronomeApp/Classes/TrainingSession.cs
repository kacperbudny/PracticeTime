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
        private readonly SoundPlayer clockHighSound;
        private readonly SoundPlayer clockLowSound;
        private readonly SoundPlayer completedSound;
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
            sessionTimer = new Stopwatch();
            IsEnabled = false;
            clockHighSound = new SoundPlayer(Resources.clock_high);
            clockLowSound = new SoundPlayer(Resources.clock_low);
            completedSound = new SoundPlayer(Resources.completed);
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
    }
}