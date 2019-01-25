using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Threading;
using static TheCrawler.LeagueEnum;

namespace TheCrawler
{
    public partial class League
    {
        public List<Match> DetermineBetability()
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
