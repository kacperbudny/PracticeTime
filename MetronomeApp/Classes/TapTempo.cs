using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MetronomeApp.Classes
{
    internal class TapTempo
    {
        public int FinalTempo { get; private set; }
        public bool IsEnabled { get; private set; }

        private readonly Stopwatch timer = new Stopwatch();
        private readonly List<long> tapTimes = new List<long>();

        public TapTempo()
        {
            FinalTempo = 0;
            IsEnabled = false;
        }

        public void Start()
        {
            IsEnabled = true;
            timer.Start();
        }

        public void RecordNextTap()
        {
            tapTimes.Add(timer.ElapsedMilliseconds);
            long averageTime = (long)Math.Round(tapTimes.Average());
            FinalTempo = (int)(60000 / averageTime);
            timer.Restart();
        }

        public void Reset()
        {
            IsEnabled = false;
            timer.Reset();
            tapTimes.Clear();
            FinalTempo = 0;
        }

        public bool AreTapTimesNotEmpty()
        {
            return tapTimes.Count > 0;
        }
    }
}
