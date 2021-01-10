using SQLite;

namespace MetronomeApp.Classes
{
    public class Exercise
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
        [NotNull]
        public bool IsInSessionMode { get; set; }
        public int SessionModeOrder { get; set; }

        [Ignore]
        public bool IsCompleted => CurrentTempo == TargetTempo ? true : false;
    }
}
