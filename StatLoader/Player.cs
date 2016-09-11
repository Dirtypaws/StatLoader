using System.Data;
using System.Security.Permissions;
using Newtonsoft.Json;

namespace StatLoader
{
    public abstract class Player
    {
        public int Id { get; set; }

        public int? Number { get; set; }

        public string Name { get; set; }

        //string[] split => Name.Split(',');
        //public string LastName => JsonConvert.ToString(split[0]);
        //public string FirstInitial => split.Length > 1 ? split[1].Replace("(Captain)", "") : "Not found";
        public bool IsCaptain => Name.Contains("(Captain)");

        public string Team { get; set; }
        public string Division { get; set; }

        public string Season { get; set; }
        public string SeasonType { get; set; }

        public int GamesPlayed { get; set; }
    }
}