using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TheCrawler
{
    public partial class Ops
    {
        public List<string> FetchMatches_FlashScoreGetArchiveList(string leagueLink)
        {
            List<string> archive = new List<string>();

            ChromeOptions co = new ChromeOptions();

            co.AddArguments("headless");

            ChromeDriver browser = new ChromeDriver(co);

            Dictionary<int, string> linksByYears = new Dictionary<int, string>();

            browser.Navigate().GoToUrl(leagueLink);

            ICollection<IWebElement> tablesList = browser.FindElements(By.ClassName("leagueTable__season"));

            foreach (var table in tablesList)
            {
                try
                {
                    archive.Add(table.FindElement(By.TagName("a")).GetAttribute("href") + "rezultate/");
                }
                catch (Exception)
                {
                    continue;
                }
            }

            browser.Dispose();

            return archive;
        }
    }
}
