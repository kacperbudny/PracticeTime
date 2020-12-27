using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MetronomeApp.Classes
{
    public static class KeyboardCommands
    {
		public static readonly RoutedUICommand ControlMetronome = new RoutedUICommand
			(
				"ControlMetronome",
				"ControlMetronome",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.S, ModifierKeys.Control)
				}
			);


	}
}
