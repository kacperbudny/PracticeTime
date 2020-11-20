using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MetronomeApp.Classes
{
    class TapTempoBinding : INotifyPropertyChanged
    {
        private bool isTapTempoEnabled;
        public bool IsTapTempoEnabled
        {
            get
            {
                return isTapTempoEnabled;
            }
            set
            {
                isTapTempoEnabled = value;
                OnPropertyChanged("IsTapTempoEnabled");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string property = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
