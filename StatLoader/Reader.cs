using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Excel;

namespace StatLoader
{
    public static class Reader
    {
        public static Tuple<IEnumerable<Skater>, IEnumerable<Goalie>> GetPlayers(string path)
        {
            var skaters = new List<Skater>();
            var goalies = new List<Goalie>();
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream))
                {
                    var data = reader.AsDataSet();

                    var id = 1;
                    foreach (
                        var row in
                            data.Tables[0].Rows.Cast<DataRow>().Where(x => !x[(int)Column.Player].ToString().StartsWith("???")).Skip(10))
                    {
                        // Parse the Games played to see if we need a Goalie, Skater or Both.
                        // If we're creating neither we'll skip the ID increment as well to tidy things up a bit.
                        int sgp, ggp;
                        int.TryParse(row[(int) Column.SGP].ToString(), out sgp);
                        int.TryParse(row[(int) Column.GoalGP].ToString(), out ggp);

                        if (sgp > 0)
                        {
                            var p = new Skater
                            {
                                Id = id,

                                Name = row[(int) Column.Player].ToString(),
                                Division = row[(int) Column.Division].ToString(),

                                Season = row[(int) Column.Season].ToString(),
                                SeasonType = row[(int) Column.SeasonType].ToString(),

                                GamesPlayed = sgp,

                                Goals = row[(int) Column.Goals].StatParse(),
                                Assists = row[(int) Column.Assists].StatParse(),
                                Points = row[(int) Column.Points].StatParse(),
                                PIMs = row[(int) Column.PIMs].StatParse(),

                                PowerPlay = row[(int) Column.PP].StatParse(),
                                ShortHanded = row[(int) Column.SH].StatParse(),
                                EvenStrength = row[(int) Column.ES].StatParse(),
                                GameWinning = row[(int) Column.GW].StatParse(),
                                GameTieing = row[(int) Column.GT].StatParse(),
                                Unassisted = row[(int) Column.UA].StatParse(),
                                PenaltyShot = row[(int) Column.SH].StatParse(),
                                EmptyNet = row[(int) Column.EN].StatParse(),

                                FirstAssist = row[(int) Column.First].StatParse(),
                                SecondAssist = row[(int) Column.Second].StatParse()
                            };

                            int number;
                            if (int.TryParse(row[0].ToString(), out number))
                                p.Number = number;
                            id ++;
                            skaters.Add(p);
                        }

                        if (ggp > 0)
                        {
                            var g = new Goalie
                            {
                                Id = id,
                                Name = row[(int) Column.Player].ToString(),

                                Division = row[(int) Column.Division].ToString(),
                                Season = row[(int) Column.Season].ToString(),
                                SeasonType = row[(int) Column.SeasonType].ToString(),

                                GoalsAgainst = row[(int) Column.GA].StatParse(),
                                Shots = row[(int) Column.Shots].StatParse(),

                                Wins = row[(int) Column.W].StatParse(),
                                Losses = row[(int) Column.L].StatParse(),
                                Ties = row[(int) Column.T].StatParse(),
                                OvertimeLosses = row[(int) Column.OTL].StatParse()
                            };

                            int number;
                            if (int.TryParse(row[0].ToString(), out number))
                                g.Number = number;

                            id++;
                            goalies.Add(g);
                        }
                    }
                }
            }

            return new Tuple<IEnumerable<Skater>, IEnumerable<Goalie>>(skaters, goalies);
        }

        static int StatParse(this object o)
        {
            int i;
            int.TryParse(o.ToString(), out i);
            return i;
        }
    }
}