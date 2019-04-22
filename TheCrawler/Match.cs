using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCrawler
{
    public enum Cote
    {
        Cota_1,
        Cota_X,
        Cota_2,
        Cota_1X,
        Cota_X2,
        Cota_12,
        Cota_F2
    }

    public class Match
    {
        public string Teams { get; set; }
        public DateTime PlayingDate { get; set; }
        public Dictionary<Cote, double> Cote { get; set; }
    }
}
