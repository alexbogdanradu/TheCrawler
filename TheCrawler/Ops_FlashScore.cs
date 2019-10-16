using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TheCrawler
{
    public partial class Ops
    {
        public async void FetchMatches_FlashScoreAsync(string link)
        {
            await Task.Run(() => FetchMatches_FlashScore(link));
        }

        public List<ArchiveMatch> FetchMatches_FlashScore(string link)
        {
            ChromeOptions co = new ChromeOptions();

            //co.AddArguments("headless");

            ChromeDriver browser = new ChromeDriver(co);

            List<ArchiveMatch> lsFootball = new List<ArchiveMatch>();

            Dictionary<int, string> linksByYears = new Dictionary<int, string>();

            browser.Navigate().GoToUrl(link);

            while (true)
            {
                try
                {
                    IWebElement button = browser.FindElement(By.XPath("//*[starts-with(@class,'event__more')]"));

                    try
                    {
                        button.Click();
                        Thread.Sleep(3000);
                    }
                    catch (Exception)
                    {

                    }
                    
                }
                catch (Exception)
                {
                    break;
                }
            }

            ICollection<IWebElement> soccerList = browser.FindElements(By.XPath("//*[starts-with(@id,'g_1_')]"));
            IWebElement WELeagueName = browser.FindElement(By.ClassName("teamHeader__name"));
            IWebElement WELeagueYear = browser.FindElement(By.ClassName("teamHeader__text"));

            int day;
            int month;
            int hour;
            int minute;
            int year;

            string date = "";

            foreach (var game in soccerList)
            {
                Stopwatch stopwatch = new Stopwatch();

                stopwatch.Start();

                IWebElement WETime = game.FindElement(By.ClassName("event__time"));
                Console.WriteLine(stopwatch.ElapsedMilliseconds + " to get the event time");
                IWebElement WEHomeTeam = game.FindElement(By.CssSelector("div[class*='event__participant event__participant--home']"));
                Console.WriteLine(stopwatch.ElapsedMilliseconds + " to get the home team");
                IWebElement WEAwayTeam = game.FindElement(By.CssSelector("div[class*='event__participant event__participant--away']"));
                Console.WriteLine(stopwatch.ElapsedMilliseconds + " to get the away team");
                IWebElement WEScore = game.FindElement(By.CssSelector("div[class*='event__scores']"));
                Console.WriteLine(stopwatch.ElapsedMilliseconds + " to get the scores");
                IWebElement WEPauza = game.FindElement(By.CssSelector("div[class*='event__part']"));
                Console.WriteLine(stopwatch.ElapsedMilliseconds + " to get the pause element");

                date = WETime.Text;

                day = Convert.ToInt32(date.Substring(0, 2));
                month = Convert.ToInt32(date.Substring(3, 2));
                hour = Convert.ToInt32(date.Substring(7, 2));
                minute = Convert.ToInt32(date.Substring(10, 2));
                year = Convert.ToInt32(WELeagueYear.Text.Substring(0, 4));
                
                Console.WriteLine(stopwatch.ElapsedMilliseconds + " to convert string to date");

                lsFootball.Add(new ArchiveMatch
                {
                    HomeTeam = WEHomeTeam.Text,
                    AwayTeam = WEAwayTeam.Text,
                    PlayingDate = new DateTime(year, month, day, hour, minute, 0),
                    HomeTeamScore = Convert.ToInt32(WEScore.Text.Substring(0, WEScore.Text.IndexOf("\r\n"))),
                    AwayTeamScore = Convert.ToInt32(WEScore.Text.Substring(WEScore.Text.IndexOf("- \r\n") + 4)),
                    League = WELeagueName.Text
                });

                Console.WriteLine(stopwatch.ElapsedMilliseconds + " to get the entire match");
            }

            using (StreamWriter sw = new StreamWriter($"results/{DateTime.Now.ToFileTime()}.json"))
            {
                sw.Write(JsonConvert.SerializeObject(lsFootball));
                sw.Flush();
                sw.Close();
            }

            browser.Dispose();

            return lsFootball;
        }
    }
}
