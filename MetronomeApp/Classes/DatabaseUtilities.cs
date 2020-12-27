using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetronomeApp.Classes
{
    public static class DatabaseUtilities
    {
        public static void UpdateExercise(Exercise exercise)
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Exercise>();
                connection.Update(exercise);
            }
        }

        public static void SwapExerciseSessionOrders(Exercise exercise, Exercise exerciseToSwap)
        {
            int exerciseSessionOrder = exercise.SessionModeOrder;
            exercise.SessionModeOrder = exerciseToSwap.SessionModeOrder;
            exerciseToSwap.SessionModeOrder = exerciseSessionOrder;

            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Exercise>();
                connection.Update(exercise);
                connection.Update(exerciseToSwap);
            }
        }
    }
}
