using SQLite;
using System.Collections.Generic;

namespace MetronomeApp.Classes
{
    public static class DatabaseUtilities
    {
        public static List<Exercise> ReadExercises()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<Exercise>();
                return conn.Table<Exercise>().ToList();
            }
        }

        public static void AddExercise(Exercise exercise)
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Exercise>();
                connection.Insert(exercise);
            }
        }

        public static void DeleteExercise(Exercise exercise)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<Exercise>();
                conn.Delete(exercise);
            }
        }

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
