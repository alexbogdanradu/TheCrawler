using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using static TheCrawler.LeagueEnum;

namespace TheCrawler
{
    public partial class Ops
    {
        public void GetPlayedMatchesFromWeb()
        {
            if (this.browser == null)
            {
                this.browser = new ChromeDriver(options);
            }

            this.InitAllPlayedMatches();

            foreach (var item in playedmatches)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                Console.Write($"Getting standings for: {item.currentLeague}. ");
                item.lsMatches.AddRange(GetPlayedGamesByLeagueFromWeb(item.currentLeague, ref browser));

                stopwatch.Stop();
                Console.WriteLine($"Page retrieval lasted {stopwatch.Elapsed.Milliseconds} mS.");
            }

            string path = $"flashscoredb_{playedmatches.Count}_playedmatches_{DateTime.Now.ToShortDateString().Replace(".", "")}.json";
            path = path.Replace("/", "");

            this.SerializeToFile(path, playedmatches);
        }

        public void GetMatchesAndStandingsFromWeb()
        {
            if (this.browser == null)
            {
                this.browser = new ChromeDriver(options);
            }

            this.InitAllLeagues();

            foreach (var item in lglist)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                Console.Write($"Getting matches for: {item.currentLeague}. ");
                item.lsMatches.AddRange(GetMatchesByLeagueFromWeb(item.currentLeague, ref browser));

                stopwatch.Stop();
                Console.WriteLine($"Page retrieval lasted {stopwatch.Elapsed.Milliseconds} mS.");

                stopwatch.Restart();
                Console.Write($"Getting standings for: {item.currentLeague}. ");
                item.lsStandings.AddRange(GetStandingsByLeagueFromWeb(item.currentLeague, ref browser));

                stopwatch.Stop();
                Console.WriteLine($"Page retrieval lasted {stopwatch.Elapsed.Milliseconds} mS.");
            }

            string path = $"flashscoredb_{this.lglist.Count}_leagues_{DateTime.Now.ToShortDateString().Replace(".", "")}.json";
            path = path.Replace("/", "");

            this.SerializeToFile(path, lglist);
        }

        private void SerializeToFile(string path, Object list)
        {
            string content = JsonConvert.SerializeObject(list);

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(content);
            }
        }

        private void InitAllLeagues()
        {
            lglist = null;
            lglist = new List<League>();

            foreach (LeagueEnum.Leagues lg in (LeagueEnum.Leagues[])Enum.GetValues(typeof(LeagueEnum.Leagues)))
            {
                lglist.Add(new League());
                lglist[lglist.Count - 1].InitLeague(lg);
            }
        }

        private void InitAllPlayedMatches()
        {
            playedmatches = null;
            playedmatches = new List<League>();

            foreach (LeagueEnum.Leagues lg in (LeagueEnum.Leagues[])Enum.GetValues(typeof(LeagueEnum.Leagues)))
            {
                playedmatches.Add(new League());
                playedmatches[playedmatches.Count - 1].InitLeague(lg);
            }
        }

        private List<Match> GetPlayedGamesByLeagueFromWeb(LeagueEnum.Leagues _league, ref ChromeDriver _driver)
        {
            if (this.browser == null)
            {
                this.browser = new ChromeDriver(options);
            }

            List<Match> matches = new List<Match>();

            int tries = triesNumber;

            while (tries-- > 1)
            {
                bool bMustRefresh = false;

                try
                {
                    _driver.Navigate().GoToUrl($"https://www.flashscore.ro/{dictLeagues[_league]}/rezultate/");

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
                        matches[iHome++].strAwayTeam = item.Text;
                    }

                    foreach (var item in away)
                    {
                        matches[iAway++].strHomeTeam = item.Text;
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
                    Thread.Sleep(50);
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

            int tries = triesNumber;

            while (tries-- > 1)
            {
                bool bMustRefresh = false;

                try
                {
                    _driver.Navigate().GoToUrl($"https://www.flashscore.ro/{dictLeagues[_league]}/clasament/");

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
                    Thread.Sleep(50);
                }
            }

            return standings;
        }

        private List<Match> GetMatchesByLeagueFromWeb(LeagueEnum.Leagues _league, ref ChromeDriver _driver)
        {
            if (this.browser == null)
            {
                this.browser = new ChromeDriver(options);
            }

            List<Match> matches = new List<Match>();

            int tries = triesNumber;

            while (tries-- > 1)
            {
                bool bMustRefresh = false;

                try
                {
                    _driver.Navigate().GoToUrl($"https://www.flashscore.ro/{dictLeagues[_league]}/meciuri/");

                    IWebElement table = _driver.FindElement(By.TagName("tbody"));

                    ICollection<IWebElement> home = table.FindElements(By.ClassName("team-home"));
                    ICollection<IWebElement> away = table.FindElements(By.ClassName("team-away"));
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
                        string matchType = dictLeagues[_league];

                        matchType = matchType.Substring(0, matchType.IndexOf("/"));
                        matches[iHome].matchLeague = _league;
                        matches[iHome].strGameType = matchType;
                        matches[iHome++].strHomeTeam = item.Text;
                    }

                    foreach (var item in away)
                    {
                        matches[iAway++].strAwayTeam = item.Text;
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
                    Thread.Sleep(50);
                }
            }

            return matches;
        }

        private void SerializeLeaguesToFile(string _path)
        {
            serializer.Serialize(new StreamWriter(_path), lglist);
        }

        private void SerializePlayedMatchesToFile(string _path)
        {
            serializer.Serialize(new StreamWriter(_path), playedmatches);
        }
    }
}
