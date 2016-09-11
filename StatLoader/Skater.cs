using Nest;

namespace StatLoader
{
    [ElasticsearchType(IdProperty = "Id")]
    public class Skater : Player
    {
        public string Type = "Skater";

        int _gamesPlayed => (GamesPlayed == 0 ? 1 : GamesPlayed);

        public int Goals { get; set; }
        public int Assists { get; set; }
        public int Points { get; set; }
        public int PIMs { get; set; }

        public decimal GoalsPerGame => Goals / _gamesPlayed;
        public decimal AssistsPerGame => Assists / _gamesPlayed;
        public decimal PointsPerGame => Assists / _gamesPlayed;
        public decimal PIMsPerGame => PIMs / _gamesPlayed;

        public int PowerPlay { get; set; }
        public int ShortHanded { get; set; }
        public int EvenStrength { get; set; }
        public int GameWinning { get; set; }
        public int GameTieing { get; set; }
        public int Unassisted { get; set; }
        public int PenaltyShot { get; set; }
        public int EmptyNet { get; set; }

        public int FirstAssist { get; set; }
        public int SecondAssist { get; set; }
    }
}