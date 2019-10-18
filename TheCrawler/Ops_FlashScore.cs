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
        public List<Task> GetListOfTasks(List<string> links)
        {
            List<Task> tasks = new List<Task>();

            foreach (var link in links)
            {
                Task task = new Task(() => FetchMatches_FlashScore(link));
                tasks.Add(task);
            }

            return tasks;
        }

        public async void FetchMatches_FlashScoreAsync(string link)
        {
            await Task.Run(() => FetchMatches_FlashScore(link));
        }

        public List<ArchiveMatch> FetchMatches_FlashScore(string link)
        {
            ChromeOptions co = new ChromeOptions();

            co.AddArguments("headless");

            ChromeDriver browser = new ChromeDriver(co);
            //ChromeDriver browser = new ChromeDriver();

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
                        ((IJavaScriptExecutor)browser).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 150)");
                        DateTime start = DateTime.Now;
                        button.Click();
                        DateTime stop1 = DateTime.Now;
                        Thread.Sleep(500);
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

            Stopwatch stopwatch = new Stopwatch();
            Stopwatch stopwatchTotal = new Stopwatch();
            stopwatch.Start();
            stopwatchTotal.Start();

            int matchCount = 0;

            foreach (var game in soccerList)
            {
                try
                {
                    matchCount++;

                    stopwatch.Restart();

                    IWebElement WETime = game.FindElement(By.ClassName("event__time"));
                    IWebElement WEHomeTeam = game.FindElement(By.CssSelector("div[class*='event__participant event__participant--home']"));
                    IWebElement WEAwayTeam = game.FindElement(By.CssSelector("div[class*='event__participant event__participant--away']"));
                    IWebElement WEScore = game.FindElement(By.CssSelector("div[class*='event__scores']"));
                    IWebElement WEPauza = game.FindElement(By.CssSelector("div[class*='event__part']"));

                    date = WETime.Text;

                    day = Convert.ToInt32(date.Substring(0, 2));
                    month = Convert.ToInt32(date.Substring(3, 2));
                    hour = Convert.ToInt32(date.Substring(7, 2));
                    minute = Convert.ToInt32(date.Substring(10, 2));
                    year = Convert.ToInt32(WELeagueYear.Text.Substring(0, 4));

                    lsFootball.Add(new ArchiveMatch
                    {
                        HomeTeam = WEHomeTeam.Text,
                        AwayTeam = WEAwayTeam.Text,
                        PlayingDate = new DateTime(year, month, day, hour, minute, 0),
                        HomeTeamScore = Convert.ToInt32(WEScore.Text.Substring(0, WEScore.Text.IndexOf("\r\n"))),
                        AwayTeamScore = Convert.ToInt32(WEScore.Text.Substring(WEScore.Text.IndexOf("- \r\n") + 4)),
                        League = WELeagueName.Text
                    });

                    //Console.WriteLine($"{matchCount} from {WELeagueName.Text}/{year} in: {stopwatch.ElapsedMilliseconds}mS.");
                }
                catch (Exception ex)
                {
                    //Console.WriteLine($"Something went wrong while getting a match {ex.Message}");
                }
            }

            Console.WriteLine($"Finished getting {WELeagueName.Text}/{WELeagueYear.Text.Substring(0, 4)} in: {stopwatch.Elapsed.TotalSeconds}S ({matchCount} total)");

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
