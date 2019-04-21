
using System.Collections.Generic;
using static TheCrawler.LeagueEnum;

namespace TheCrawler
{
    public partial class League
    {
        public List<Match> DetermineBetability(Algos algos)
        {
            switch (algos)
            {
                case Algos.OiganBet:
                    return OiganBetAlgo();
                case Algos.CristiBet:
                    return CristiBetAlgo();
                default:
                    return null;
            }
        }

        private List<Match> CristiBetAlgo()
        {
            return null;
        }

        private List<Match> OiganBetAlgo()
        {
            List<Match> results = new List<Match>();

            int standingsIndex = -1;

            if (this.lsStandings.Count < 12)
            {
                standingsIndex = 2;
            }
            else
            {
                if (this.lsStandings.Count < 16)
                {
                    standingsIndex = 3;
                }
                else
                {
                    standingsIndex = 4;
                }
            }

            foreach (var item in lsMatches)
            {
                if (lsStandings.IndexOf(item.strHomeTeam) < standingsIndex)
                {
                    if (lsStandings.IndexOf(item.strAwayTeam) > lsStandings.Count - standingsIndex)
                    {
                        item.strResultEstimated = "1";
                    }
                }

                if (lsStandings.IndexOf(item.strHomeTeam) > lsStandings.Count - standingsIndex)
                {
                    if (lsStandings.IndexOf(item.strAwayTeam) < standingsIndex)
                    {
                        item.strResultEstimated = "2";
                    }
                }
            }

            foreach (var item in lsMatches)
            {
                if (item.strResultEstimated != null)
                {
                    results.Add(item);
                }
            }

            return results;
        }
    }
}
