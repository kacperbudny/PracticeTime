using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MetronomeApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static readonly string databaseName = "Exercises.db";
        static readonly string folderName = "MetronomeApp";
        static readonly string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string applicationDirectoryPath = System.IO.Path.Combine(folderPath, folderName);
        public static string databasePath = System.IO.Path.Combine(folderPath, folderName, databaseName);
    }
}
