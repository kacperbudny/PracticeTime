using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetronomeApp.Classes
{
    static class SilencePlayer
    {
        public static void Play()
        {
            var silence = new SilenceProvider(new WaveFormat()).ToSampleProvider();
            WaveOutEvent wo = new WaveOutEvent();
            wo.Init(silence);
            wo.Play();
        }
    }
}
