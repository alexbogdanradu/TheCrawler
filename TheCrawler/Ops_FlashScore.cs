using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
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
            string date = DateTime.Now.ToString("dd.MM.yyyy");

 
            ChromeOptions co = new ChromeOptions();

            co.AddArguments("headless");

            ChromeDriver browser = new ChromeDriver(co);

            List<ArchiveMatch> lsFootball = new List<ArchiveMatch>();

            Dictionary<int, string> linksByYears = new Dictionary<int, string>();

           
            browser.Navigate().GoToUrl(link);

            ICollection<IWebElement> soccerList = browser.FindElements(By.XPath("//*[starts-with(@id,'g_1_')]"));

            foreach (var game in soccerList)
            {
                List<ArchiveMatch> localMatches = new List<ArchiveMatch>();

                IWebElement WETime = game.FindElement(By.ClassName("event__time"));
                IWebElement WEHomeTeam = game.FindElement(By.CssSelector("div[class*='event__participant event__participant--home']"));
                IWebElement WEAwayTeam = game.FindElement(By.CssSelector("div[class*='event__participant event__participant--away']"));
                IWebElement WEScore = game.FindElement(By.CssSelector("div[class*='event__scores']"));
                IWebElement WEPauza = game.FindElement(By.CssSelector("div[class*='event__part']"));

                localMatches.Add(new ArchiveMatch
                {
                    HomeTeam = WEHomeTeam.Text,
                    AwayTeam = WEAwayTeam.Text,
                    //PlayingDate = DateTime.ParseExact(WETime.Text, "", null),
                    HomeTeamScore = Convert.ToInt32(WEScore.Text.Substring(0, WEScore.Text.IndexOf("\r\n"))),
                    AwayTeamScore = Convert.ToInt32(WEScore.Text.Substring(WEScore.Text.IndexOf("- \r\n") + 4)),
                });


                lsFootball.AddRange(localMatches);
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
