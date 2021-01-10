using System;
using System.Windows;

namespace MetronomeApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly string databaseName = "Exercises.db";
        private static readonly string folderName = "MetronomeApp";
        private static readonly string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string applicationDirectoryPath = System.IO.Path.Combine(folderPath, folderName);
        public static string databasePath = System.IO.Path.Combine(folderPath, folderName, databaseName);
    }
}
