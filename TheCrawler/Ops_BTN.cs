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
        public List<Match> FetchMatches_BTN()
        {
            string date = DateTime.Now.ToString("dd.MM.yyyy");

            ChromeDriver browser = new ChromeDriver();

            List<Match> lsFootball = new List<Match>();

            browser.Navigate().GoToUrl($"https://ro.betano.com/Upcoming24H/Soccer-FOOT/");

            ICollection<IWebElement> soccerList = browser.FindElements(By.ClassName("al8"));

            foreach (var game in soccerList)
            {
                

                List<Match> localMatches = new List<Match>();
            }
            return null;
        }
    }
}
