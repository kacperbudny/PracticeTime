﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetronomeApp.Classes
{
    class TapTempo
    {
        public int FinalTempo { get; private set; }
        public bool IsTapTempoModeEnabled { get; private set; }

        readonly Stopwatch timer = new Stopwatch();
        readonly List<long> tapTimes = new List<long>();
        
        public TapTempo()
        {
            FinalTempo = 0;
            IsTapTempoModeEnabled = false;
        }

        public void Start()
        {
            IsTapTempoModeEnabled = true;
            timer.Start();
        }

        public void NextTap()
        {
            tapTimes.Add(timer.ElapsedMilliseconds);
            long averageTime = (long)Math.Round(tapTimes.Average());
            FinalTempo = (int)(60000 / averageTime);
            timer.Restart();
        }

        public void Reset()
        {
            IsTapTempoModeEnabled = false;
            timer.Reset();
            tapTimes.Clear();
            FinalTempo = 0;
        }

        public bool AreTapTimesNotEmpty()
        {
            if (tapTimes.Count == 1) return true;
            else return false;
        }
    }
}