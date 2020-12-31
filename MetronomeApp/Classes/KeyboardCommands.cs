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
                "Start/stop metronome",
                "ControlMetronome",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.S, ModifierKeys.Control, "Ctrl + S")
                }
            );

        public static readonly RoutedUICommand Plus1 = new RoutedUICommand
            (
                "Increase tempo by 1",
                "Plus1",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.D1, ModifierKeys.Control, "Ctrl + 1")
                }
            );

        public static readonly RoutedUICommand Plus5 = new RoutedUICommand
            (
                "Increase tempo by 5",
                "Plus5",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.D2, ModifierKeys.Control, "Ctrl + 2")
                }
            );

        public static readonly RoutedUICommand Plus10 = new RoutedUICommand
            (
                "Increase tempo by 10",
                "Plus10",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.D3, ModifierKeys.Control, "Ctrl + 3")
                }
            );

        public static readonly RoutedUICommand Minus1 = new RoutedUICommand
            (
                "Decrease tempo by 1",
                "Minus1",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.D1, ModifierKeys.Alt, "Alt + 1")
                }
            );

        public static readonly RoutedUICommand Minus5 = new RoutedUICommand
            (
                "Decrease tempo by 5",
                "Minus5",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.D2, ModifierKeys.Alt, "Alt + 2")
                }
            );

        public static readonly RoutedUICommand Minus10 = new RoutedUICommand
            (
                "Decrease tempo by 10",
                "Minus10",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.D3, ModifierKeys.Alt, "Alt + 3")
                }
            );

        public static readonly RoutedUICommand TapTempoStart = new RoutedUICommand
            (
                "Tap Tempo - start",
                "TapTempoStart",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.T, ModifierKeys.Control, "Ctrl + T")
                }
            );

        public static readonly RoutedUICommand TapTempoCancel = new RoutedUICommand
            (
                "Tap Tempo - cancel",
                "TapTempoCancel",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.Y, ModifierKeys.Alt, "Alt + Y")
                }
            );

        public static readonly RoutedUICommand TapTempoAccept = new RoutedUICommand
            (
                "Tap Tempo - accept",
                "TapTempoAccept",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.T, ModifierKeys.Alt, "Alt + T")
                }
            );

        public static readonly RoutedUICommand ControlTimer = new RoutedUICommand
            (
                "Start/stop timer",
                "ControlTimer",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                            new KeyGesture(Key.S, ModifierKeys.Alt, "Alt + S")
                }
            );

        public static readonly RoutedUICommand ResetTimer = new RoutedUICommand
            (
                "Reset timer",
                "ResetTimer",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                            new KeyGesture(Key.D, ModifierKeys.Alt, "Alt + D")
                }
            );

        public static readonly RoutedUICommand PlusTimer = new RoutedUICommand
            (
                "Timer - +1 minute",
                "PlusTimer",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.Add, ModifierKeys.Control, "Ctrl + +")
                }
            );

        public static readonly RoutedUICommand MinusTimer = new RoutedUICommand
            (
                "Timer - -1 minute",
                "MinusTimer",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.Subtract, ModifierKeys.Control, "Ctrl + -")
                }
            );

        public static readonly RoutedUICommand GoToSearch = new RoutedUICommand
            (
                "Focus on the search bar",
                "GoToSearch",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                            new KeyGesture(Key.F11, ModifierKeys.None, "F11")
                }
            );

        public static readonly RoutedUICommand Unfocus = new RoutedUICommand
            (
                "Unfocus from the search bar",
                "Unfocus",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.Escape, ModifierKeys.None, "Esc")
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
                "Change exercise list's view",
                "ChangeView",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.F1, ModifierKeys.None, "F1")
                }
            );

        public static readonly RoutedUICommand GoToListview = new RoutedUICommand
            (
                "Focus on the exercise list",
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

        public static readonly RoutedUICommand SwapWithPrevious = new RoutedUICommand
            (
                "Swap exercise with previous one",
                "SwapWithPrevious",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                            new KeyGesture(Key.OemOpenBrackets, ModifierKeys.Control, "Ctrl + [")
                }
            );

        public static readonly RoutedUICommand SwapWithNext = new RoutedUICommand
            (
                "Swap exercise with next one",
                "SwapWithNext",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.OemCloseBrackets, ModifierKeys.Control, "Ctrl + ]")
                }
            );

        public static readonly RoutedUICommand ControlSession = new RoutedUICommand
            (
                "Start/stop Session Mode",
                "ControlSession",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.F6, ModifierKeys.None, "F6")
                }
            );

        public static readonly RoutedUICommand PreviousExercise = new RoutedUICommand
            (
                "Session Mode - go to previous exercise",
                "PreviousExercise",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.D9, ModifierKeys.Control, "Ctrl + 9")
                }
            );

        public static readonly RoutedUICommand NextExercise = new RoutedUICommand
            (
                "Session Mode - go to next exercise",
                "NextExercise",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.D0, ModifierKeys.Control, "Ctrl + 0")
                }
            );

        public static readonly RoutedUICommand AcceptCongratulations = new RoutedUICommand
            (
                "Accept congratulations after session",
                "AcceptCongratulations",
                typeof(KeyboardCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.F12, ModifierKeys.None, "F12")
                }
            );
    }
}
