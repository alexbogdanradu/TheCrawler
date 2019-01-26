using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static TheCrawler.LeagueEnum;

namespace TheCrawler
{
    public partial class Ops
    {
        public void SerializeToFile(string path)
        {
            serializer.Serialize(new StreamWriter(path), lglist);
        }

        public void InitSpecificLeague(LeagueEnum.Leagues league)
        {
            lglist.Add(new League());
            lglist[lglist.Count - 1].InitLeague(league);
        }

        public void InitAllLeagues()
        {
            foreach (LeagueEnum.Leagues lg in (LeagueEnum.Leagues[])Enum.GetValues(typeof(LeagueEnum.Leagues)))
            {
                lglist.Add(new League());
                playedmatches.Add(new League());
                lglist[lglist.Count - 1].InitLeague(lg);
                playedmatches[playedmatches.Count - 1].InitLeague(lg);
            }
        }

        public void InitAllPlayedMatches()
        {
            foreach (LeagueEnum.Leagues lg in (LeagueEnum.Leagues[])Enum.GetValues(typeof(LeagueEnum.Leagues)))
            {
                playedmatches.Add(new League());
                playedmatches[playedmatches.Count - 1].InitLeague(lg);
            }
        }

        public void GetLatestMatchesAndStandingsFromWeb(ref ChromeDriver _driver)
        {
            if (this.browser == null)
            {
                this.browser = new ChromeDriver(options);
            }

            foreach (var item in this.lglist)
            {
                item.lsMatches.AddRange(GetGamesByLeagueFromWeb(item.currentLeague, ref _driver));
                item.lsStandings.AddRange(GetStandingsByLeagueFromWeb(item.currentLeague, ref _driver));
            }
        }

        public List<Match> FillActualScore(List<Match> res2)
        {
            List<Match> result = new List<Match>();

            foreach (var match2 in res2)
            {
                if (match2.iAwayScore != match2.iHomeScore)
                {
                    if (match2.iHomeScore > match2.iAwayScore)
                    {
                        match2.strResultActual = "1";
                        result.Add(match2);
                    }
                    else
                    {
                        match2.strResultActual = "2";
                        result.Add(match2);
                    }
                }
                else
                {
                    match2.strResultActual = "-1";
                    result.Add(match2);
                }
            }

            return result;
        }

        public void GetPlayedMatchesFromWeb(ref ChromeDriver _driver)
        {
            if (this.browser == null)
            {
                this.browser = new ChromeDriver(options);
            }

            foreach (var item in this.playedmatches)
            {
                item.lsMatches.AddRange(GetPlayedGamesByLeagueFromWeb(item.currentLeague, ref _driver));
            }
        }

        private List<Match> GetPlayedGamesByLeagueFromWeb(LeagueEnum.Leagues _league, ref ChromeDriver _driver)
        {
            if (this.browser == null)
            {
                this.browser = new ChromeDriver(options);
            }

            List<Match> matches = new List<Match>();

            int tries = 1000;

            while (tries-- > 1)
            {
                bool bMustRefresh = false;

                try
                {
                    _driver.Navigate().GoToUrl($"https://www.flashscore.ro/fotbal/{dictLeagues[_league]}/rezultate/");

                    IWebElement table = _driver.FindElement(By.TagName("tbody"));

                    ICollection<IWebElement> home = table.FindElements(By.ClassName("padl"));
                    ICollection<IWebElement> away = table.FindElements(By.ClassName("padr"));
                    ICollection<IWebElement> score = table.FindElements(By.ClassName("score"));
                    ICollection<IWebElement> time = table.FindElements(By.ClassName("time"));

                    int iHome = 0;
                    int iAway = 0;
                    int iScore = 0;
                    int iTime = 0;

                    foreach (var item in home)
                    {
                        matches.Add(new Match());
                    }

                    foreach (var item in home)
                    {
                        matches[iHome++].strHomeTeam = item.Text;
                    }

                    foreach (var item in away)
                    {
                        matches[iAway++].strAwayTeam = item.Text;
                    }

                    foreach (var item in score)
                    {
                        string _sScore = item.Text;
                        _sScore = _sScore.Replace(" ", "");
                        matches[iScore].iHomeScore = Convert.ToInt32(_sScore.Substring(0, _sScore.IndexOf(":")));
                        matches[iScore].iAwayScore = Convert.ToInt32(_sScore.Substring(_sScore.IndexOf(":") + 1, _sScore.Length - _sScore.IndexOf(":") - 1));
                        iScore++;
                    }

                    foreach (var item in time)
                    {
                        string datestring = item.Text;
                        int day = Convert.ToInt32(datestring.Substring(0, datestring.IndexOf('.')));
                        datestring = datestring.Substring(datestring.IndexOf('.') + 1);

                        int month = Convert.ToInt32(datestring.Substring(0, datestring.IndexOf('.')));
                        datestring = datestring.Substring(datestring.IndexOf('.') + 2);

                        int hour = Convert.ToInt32(datestring.Substring(0, datestring.IndexOf(':')));
                        datestring = datestring.Substring(datestring.IndexOf(':') + 1);

                        int minute = Convert.ToInt32(datestring);

                        matches[iTime++].dtDateTime = new DateTime(DateTime.Now.Year, month, day, hour, minute, 0);
                    }

                    bMustRefresh = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    bMustRefresh = true;
                }

                if (!bMustRefresh)
                {
                    _driver.Manage().Cookies.DeleteAllCookies();
                    break;
                }
                else
                {
                    Thread.Sleep(500);
                }
            }

            return matches;
        }

        private List<string> GetStandingsByLeagueFromWeb(LeagueEnum.Leagues _league, ref ChromeDriver _driver)
        {
            if (this.browser == null)
            {
                this.browser = new ChromeDriver(options);
            }

            List<string> standings = new List<string>();

            int tries = 1000;

            while (tries-- > 1)
            {
                bool bMustRefresh = false;

                try
                {
                    _driver.Navigate().GoToUrl($"https://www.flashscore.ro/fotbal/{dictLeagues[_league]}/clasament/");

                    IWebElement table = _driver.FindElement(By.TagName("tbody"));
                    ICollection<IWebElement> team = table.FindElements(By.ClassName("team_name_span"));

                    foreach (var item in team)
                    {
                        standings.Add(item.Text);
                    }

                    bMustRefresh = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    bMustRefresh = true;
                }

                if (!bMustRefresh)
                {
                    _driver.Manage().Cookies.DeleteAllCookies();
                    break;
                }
                else
                {
                    Thread.Sleep(500);
                }
            }

            return standings;
        }

        private List<Match> GetGamesByLeagueFromWeb(LeagueEnum.Leagues _league, ref ChromeDriver _driver)
        {
            if (this.browser == null)
            {
                this.browser = new ChromeDriver(options);
            }

            List<Match> matches = new List<Match>();

            int tries = 1000;

            while (tries-- > 1)
            {
                bool bMustRefresh = false;

                try
                {
                    _driver.Navigate().GoToUrl($"https://www.flashscore.ro/fotbal/{dictLeagues[_league]}/meciuri/");

                    IWebElement table = _driver.FindElement(By.TagName("tbody"));

                    ICollection<IWebElement> home = table.FindElements(By.ClassName("padl"));
                    ICollection<IWebElement> away = table.FindElements(By.ClassName("padr"));
                    ICollection<IWebElement> time = table.FindElements(By.ClassName("time"));

                    int iHome = 0;
                    int iAway = 0;
                    int iTime = 0;

                    foreach (var item in home)
                    {
                        matches.Add(new Match());
                    }

                    foreach (var item in home)
                    {
                        matches[iHome++].strAwayTeam = item.Text;
                    }

                    foreach (var item in away)
                    {
                        matches[iAway++].strHomeTeam = item.Text;
                    }

                    foreach (var item in time)
                    {
                        string datestring = item.Text;
                        int day = Convert.ToInt32(datestring.Substring(0, datestring.IndexOf('.')));
                        datestring = datestring.Substring(datestring.IndexOf('.') + 1);

                        int month = Convert.ToInt32(datestring.Substring(0, datestring.IndexOf('.')));
                        datestring = datestring.Substring(datestring.IndexOf('.') + 2);

                        int hour = Convert.ToInt32(datestring.Substring(0, datestring.IndexOf(':')));
                        datestring = datestring.Substring(datestring.IndexOf(':') + 1);

                        int minute = Convert.ToInt32(datestring);

                        matches[iTime++].dtDateTime = new DateTime(DateTime.Now.Year, month, day, hour, minute, 0);
                    }

                    bMustRefresh = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    bMustRefresh = true;
                }

                if (!bMustRefresh)
                {
                    _driver.Manage().Cookies.DeleteAllCookies();
                    break;
                }
                else
                {
                    Thread.Sleep(500);
                }
            }

            return matches;
        }

        public void GetLatestMatchesAndStandingsFromFile(string _file)
        {
            StreamReader streamReader = new StreamReader(_file);
            lglist.AddRange((List<League>)serializer.Deserialize(streamReader));
        }

        public void GetPlayedMatchesFromFile(string _file)
        {
            StreamReader streamReader = new StreamReader(_file);
            playedmatches.AddRange((List<League>)serializer.Deserialize(streamReader));
        }

        public void SerializeLeaguesToFile(string _path)
        {
            serializer.Serialize(new StreamWriter(_path), lglist);
        }

        public void SerializePlayedMatchesToFile(string _path)
        {
            serializer.Serialize(new StreamWriter(_path), playedmatches);
        }

        public void SerializeToFiles()
        {

        }

        public List<Match> DetermineBetabilityForFutureMatches()
        {
            List<Match> result = new List<Match>();

            foreach (var league in this.lglist)
            {
                if (league.lsStandings.Count != 0)
                {
                    result.AddRange(league.DetermineBetability());
                }


            }

            return result;
        }

        public List<Match> DetermineBetabilityForPastMatches(List<League> standings)
        {
            List<Match> result = new List<Match>();

            foreach (var played in playedmatches)
            {
                foreach (var notplayed in standings)
                {
                    if (notplayed.currentLeague == played.currentLeague)
                    {
                        played.lsStandings.AddRange(notplayed.lsStandings);
                    }
                }
            }

            foreach (var league in this.playedmatches)
            {
                if (league.lsStandings.Count != 0)
                {
                    league.DetermineBetability();

                    foreach (var match in league.lsMatches)
                    {
                        if (match.strResultEstimated != "-1")
                        {
                            result.Add(match);
                        }
                    }
                }
            }

            return result;
        }

        public void Clean()
        {
            browser.Dispose();
        }

        public void ShowFutureWinningMatchesByDaysFromNow(int days, List<Match> res1)
        {
            int i = 1;
            int cursory = 0;
            DateTime today = DateTime.Now;

            Console.WriteLine($"|-GAMES-IN THE NEXT-{days}-DAYS---------------------------------------------------------------------------|");
            
            cursory++;

            foreach (var match in res1)
            {
                TimeSpan offset = new TimeSpan(days, 0, 0, 0);
                DateTime date = DateTime.Now.Date;

                if (days == -1)
                {
                    Console.WriteLine($"| {i++}. {match.strHomeTeam}  vs {match.strAwayTeam}. Estimated: {match.strResultEstimated}. {match.dtDateTime}");
                }
                else
                {
                    if ((match.dtDateTime > DateTime.Now) && (match.dtDateTime < date + offset))
                    {
                        Console.SetCursorPosition(0, cursory);
                        Console.Write($"| {i++}. ");
                        Console.SetCursorPosition(4, cursory);
                        Console.Write($" {match.strHomeTeam} ");
                        Console.SetCursorPosition(30, cursory);
                        Console.Write($" vs {match.strAwayTeam} ");
                        Console.SetCursorPosition(60, cursory);
                        Console.Write($" Estimated: {match.strResultEstimated}. ");
                        Console.SetCursorPosition(80, cursory);
                        Console.Write($"{match.dtDateTime} -|");
                        cursory++;
                    }
                }
            }

            Console.SetCursorPosition(0, cursory);
            Console.WriteLine("|-END------------------------------------------------------------------------------------------------|");
        }
    }
}
