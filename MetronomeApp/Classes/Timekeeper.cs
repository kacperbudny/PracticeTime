using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MetronomeApp.Classes
{
    class Timekeeper
    {
        private DispatcherTimer dt;
        private int time = 0;

        public Timekeeper()
        {
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += dtTicker;
            dt.Start();
        }

        private void dtTicker(object sender, EventArgs e)
        {
            time--;
        }


    }
}
