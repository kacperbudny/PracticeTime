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
					new KeyGesture(Key.S, ModifierKeys.Control, "Ctrl + S")
				}
			);

		public static readonly RoutedUICommand Plus1 = new RoutedUICommand
			(
				"Plus1",
				"Plus1",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.D1, ModifierKeys.Control, "Ctrl + 1")
				}
			);

		public static readonly RoutedUICommand Plus5 = new RoutedUICommand
			(
				"Plus5",
				"Plus5",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.D2, ModifierKeys.Control, "Ctrl + 2")
				}
			);

		public static readonly RoutedUICommand Plus10 = new RoutedUICommand
			(
				"Plus10",
				"Plus10",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.D3, ModifierKeys.Control, "Ctrl + 3")
				}
			);

		public static readonly RoutedUICommand Minus1 = new RoutedUICommand
			(
				"Minus1",
				"Minus1",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.D1, ModifierKeys.Alt, "Alt + 1")
				}
			);

		public static readonly RoutedUICommand Minus5 = new RoutedUICommand
			(
				"Minus5",
				"Minus5",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.D2, ModifierKeys.Alt, "Alt + 2")
				}
			);

		public static readonly RoutedUICommand Minus10 = new RoutedUICommand
			(
				"Minus10",
				"Minus10",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.D3, ModifierKeys.Alt, "Alt + 3")
				}
			);

		public static readonly RoutedUICommand TapTempoStart = new RoutedUICommand
			(
				"TapTempoStart",
				"TapTempoStart",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.T, ModifierKeys.Control, "Ctrl + T")
				}
			);

		public static readonly RoutedUICommand TapTempoCancel = new RoutedUICommand
			(
				"TapTempoCancel",
				"TapTempoCancel",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.Y, ModifierKeys.Alt, "Alt + Y")
				}
			);

		public static readonly RoutedUICommand TapTempoAccept = new RoutedUICommand
			(
				"TapTempoAccept",
				"TapTempoAccept",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.T, ModifierKeys.Alt, "Alt + T")
				}
			);

		public static readonly RoutedUICommand PlusTimer = new RoutedUICommand
			(
				"PlusTimer",
				"PlusTimer",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.Add, ModifierKeys.Control, "Ctrl + +")
				}
			);

		public static readonly RoutedUICommand MinusTimer = new RoutedUICommand
			(
				"MinusTimer",
				"MinusTimer",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.Subtract, ModifierKeys.Control, "Ctrl + -")
				}
			);

		public static readonly RoutedUICommand ControlTimer = new RoutedUICommand
			(
				"ControlTimer",
				"ControlTimer",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.S, ModifierKeys.Alt, "Alt + S")
				}
			);

		public static readonly RoutedUICommand ResetTimer = new RoutedUICommand
			(
				"ResetTimer",
				"ResetTimer",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.D, ModifierKeys.Alt, "Alt + D")
				}
			);

		public static readonly RoutedUICommand AddExercise = new RoutedUICommand
			(
				"Add exercise",
				"AddExercise",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.A, ModifierKeys.Control, "Ctrl + A")
				}
			);

		public static readonly RoutedUICommand EditExercise = new RoutedUICommand
			(
				"Edit exercise",
				"EditExercise",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.E, ModifierKeys.Control, "Ctrl + E")
				}
			);

		public static readonly RoutedUICommand DeleteExercise = new RoutedUICommand
			(
				"Delete exercise",
				"DeleteExercise",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.Delete, ModifierKeys.None, "Del")
				}
			);

		public static readonly RoutedUICommand RefreshList = new RoutedUICommand
			(
				"Refresh list",
				"RefreshList",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.R, ModifierKeys.Control, "Ctrl + R")
				}
			);

		public static readonly RoutedUICommand ChangeView = new RoutedUICommand
			(
				"ChangeView",
				"ChangeView",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.F1, ModifierKeys.None, "F1")
				}
			);

		public static readonly RoutedUICommand GoToListview = new RoutedUICommand
			(
				"GoToListview",
				"GoToListview",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.F2, ModifierKeys.None, "F2")
				}
			);

		public static readonly RoutedUICommand AddToSession = new RoutedUICommand
			(
				"Add to/delete from session",
				"AddToSession",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.F3, ModifierKeys.None, "F3")
				}
			);

		public static readonly RoutedUICommand ControlSession = new RoutedUICommand
			(
				"ControlSession",
				"ControlSession",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.F6, ModifierKeys.None, "F6")
				}
			);

		public static readonly RoutedUICommand PreviousExercise = new RoutedUICommand
			(
				"PreviousExercise",
				"PreviousExercise",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.D9, ModifierKeys.Control, "Ctrl + 9")
				}
			);

		public static readonly RoutedUICommand NextExercise = new RoutedUICommand
			(
				"NextExercise",
				"NextExercise",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.D0, ModifierKeys.Control, "Ctrl + 0")
				}
			);

		public static readonly RoutedUICommand SetUpMetronomeForSelected = new RoutedUICommand
			(
				"Set up metronome for the exercise",
				"SetUpMetronomeForSelected",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.F4, ModifierKeys.None, "F4")
				}
			);

		public static readonly RoutedUICommand SaveMetronomesTempo = new RoutedUICommand
			(
				"Save current metronome's tempo into the exercise",
				"SaveMetronomesTempo",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.F5, ModifierKeys.None, "F5")
				}
			);

		public static readonly RoutedUICommand GoToSearch = new RoutedUICommand
			(
				"GoToSearch",
				"GoToSearch",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.F11, ModifierKeys.None, "F11")
				}
			);

		public static readonly RoutedUICommand SwapWithPrevious = new RoutedUICommand
			(
				"Swap exercise with previous one in session mode",
				"SwapWithPrevious",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.OemOpenBrackets, ModifierKeys.Control, "Ctrl + [")
				}
			);

		public static readonly RoutedUICommand SwapWithNext = new RoutedUICommand
			(
				"Swap exercise with next one in session mode",
				"SwapWithNext",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.OemCloseBrackets, ModifierKeys.Control, "Ctrl + ]")
				}
			);

		public static readonly RoutedUICommand Unfocus = new RoutedUICommand
			(
				"Unfocus",
				"Unfocus",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.Escape, ModifierKeys.None, "Esc")
				}
			);

		public static readonly RoutedUICommand AcceptCongratulations = new RoutedUICommand
			(
				"AcceptCongratulations",
				"AcceptCongratulations",
				typeof(KeyboardCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.F12, ModifierKeys.None, "F12")
				}
			);
	}
}
