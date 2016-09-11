namespace StatLoader
{
    public class Goalie : Player
    {
        public string Type = "Goalie";

        int _gamesPlayed => (GamesPlayed == 0 ? 1 : GamesPlayed);

        public int GoalsAgainst { get; set; }
        public decimal GoalsAgainstAvg => GoalsAgainst / _gamesPlayed;

        int shots => Shots == 0 ? 1 : Shots;
        public int Shots { get; set; }
        public decimal SavePct => GoalsAgainst / shots;

        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Ties { get; set; }
        public int OvertimeLosses { get; set; }
    }
}