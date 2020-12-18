using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetronomeApp.Classes
{
    class Exercise
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique, NotNull]
        public string Name { get; set; }
        [NotNull]
        public int StartingTempo { get; set; }
        [NotNull]
        public int TargetTempo { get; set; }
        public int CurrentTempo { get; set; }
        [NotNull]
        public int PracticeTime { get; set; }
        public string Notes { get; set; }
    }
}
