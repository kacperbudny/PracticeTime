﻿namespace MetronomeApp.Classes
{
    internal class TimekeeperHelper
    {
        private readonly AudioPlayer timekeeperSound;
        private int initialTime;
        private int _time;
        public int Time
        {
            get => _time;

            set
            {
                if (value > 3600)
                {
                    _time = 3600;
                }
                else
                {
                    _time = value;
                }
            }
        }
        public bool IsEnabled { get; private set; }

        public TimekeeperHelper()
        {
            Time = 300;
            IsEnabled = false;
            timekeeperSound = new AudioPlayer(SoundType.Timekeeper);
        }

        public void SetInitialTime()
        {
            if (!IsEnabled)
            {
                initialTime = Time;
                IsEnabled = true;
            }
        }

        public void Reset()
        {
            timekeeperSound.Reset();
            Time = initialTime;
            IsEnabled = false;
        }

        public void Complete()
        {
            Reset();
            timekeeperSound.Play();
        }

        public string GetMinutes()
        {
            return (Time / 60) >= 10 ? (Time / 60).ToString() : "0" + (Time / 60).ToString();
        }

        public string GetSeconds()
        {
            return (Time % 60) >= 10 ? (Time % 60).ToString() : "0" + (Time % 60).ToString();
        }

        public string GetFullTime()
        {
            return string.Format("{0}:{1}", GetMinutes(), GetSeconds());
        }

        public void SetVolume(float newVolume)
        {
            timekeeperSound.SetVolume(newVolume);
        }
    }
}
