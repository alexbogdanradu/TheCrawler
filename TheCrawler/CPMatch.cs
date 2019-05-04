using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCrawler
{
    public class CPMatch : Match
    {
        public string BetMin_Key { get; set; }
        public double BetMin_Val { get; set; }
        public double Bet01_DiffX1 { get; set; }
        public double Bet01_Diff12 { get; set; }
        public double Bet01_Diff2X { get; set; }
        public List<double> Cote { get; set; }
        public string Bet02_Key { get; internal set; }
        public double Bet02_Val { get; internal set; }
        public double MatchPercent { get; set; }
    }
}
