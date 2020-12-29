using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace MetronomeApp.Classes
{
    class TimekeeperHelper
    {
        readonly SoundPlayer completedSound = new SoundPlayer(Properties.Resources.timekeeper);
        private int timeToReturn;
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
                else _time = value;
            } 
        }
        public bool IsEnabled { get; private set; }

        public TimekeeperHelper()
        {
            Time = 300;
            IsEnabled = false;
        }

        public void SetTimeToReturn()
        {
            if(!IsEnabled)
            {
                timeToReturn = Time;
                IsEnabled = true;
            }
        }

        public void Reset()
        {
            Time = timeToReturn;
            IsEnabled = false;
        }

        public void Complete()
        {
            Reset();
            completedSound.Play();
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
    }
}
