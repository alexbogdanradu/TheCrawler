using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TheCrawler
{
    public partial class Ops
    {
        public List<Match> FetchMatches_SB()
        {
            ChromeDriver browser = new ChromeDriver();

            List<Match> lsFootball = new List<Match>();

            browser.Navigate().GoToUrl($"https://agentii.stanleybet.ro/sportsbetting/Fotbal/68");

            Thread.Sleep(3000);

            IWebElement button = browser.FindElement(By.CssSelector("[class='successButton ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only']"));

            button.Clear();

            IWebElement games = browser.FindElement(By.CssSelector("[class='oddsTable drop-shadow lifted']"));

            browser.Dispose();

            return lsFootball;
        }
    }
}
