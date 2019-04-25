using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCrawler
{
    public class Match
    {
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string PlayingDate { get; set; }
        public Dictionary<string, double> Dict { get; set; }
        public Dictionary<string, double> Bets { get; set; }
        public List<double> Cote { get; set; }
        public string Bet01_Key { get; set; }
        public double Bet01_Val { get; set; }
        public double Bet01_DiffX1 { get; set; }
        public double Bet01_Diff12 { get; set; }
        public double Bet01_Diff2X { get; set; }
    }
}
