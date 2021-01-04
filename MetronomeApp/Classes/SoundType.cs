using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetronomeApp.Classes
{
    public class SoundType
    {
        public static string Metronome { get { return "metronome"; } }
        public static string Timekeeper { get { return "timekeeper"; } }
        public static string Completed { get { return "completed"; } }
        public static string ClockHigh { get { return "clock-high"; } }
        public static string ClockLow { get { return "clock-low"; } }
    }
}

