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
        public List<Match> FetchMatches_NB()
        {
            string date = DateTime.Now.ToString("dd.MM.yyyy");

            ChromeDriver browser = new ChromeDriver();

            List<Match> lsFootball = new List<Match>();

            browser.Navigate().GoToUrl($"https://sport.netbet.ro/fotbal/");

            Thread.Sleep(30000);

            IWebElement games = browser.FindElement(By.ClassName("events-container"));

            ICollection<IWebElement> game = browser.FindElements(By.ClassName("event-wrapper-inner"));

            foreach (var item in game)
            {
                IWebElement HomeTeam = item.FindElement(By.CssSelector("[class='event-details-row event-details-row-team-a clearfix']"));
                IWebElement AwayTeam = item.FindElement(By.CssSelector("[class='event-details-row event-details-row-team-b clearfix']"));

                ICollection<IWebElement> odds = item.FindElements(By.CssSelector("[class='bet-button-wrap counted']"));

                IWebElement odd_1;
                IWebElement odd_X;
                IWebElement odd_2;

                int iterator = 0;
                foreach (var odd in odds)
                {
                    switch (iterator)
                    {
                        case 0:
                            odd_1 = odd.FindElement(By.CssSelector("[class='bet-odds-number']"));
                            break;
                        case 1:
                            odd_X = odd.FindElement(By.CssSelector("[class='bet-odds-number']"));
                            break;
                        case 2:
                            odd_2 = odd.FindElement(By.CssSelector("[class='bet-odds-number']"));
                            break;
                        default:
                            break;
                    }

                    iterator++;
                }

                lsFootball.Add(new Match());

                lsFootball.Last().HomeTeam = HomeTeam.Text;
                lsFootball.Last().AwayTeam = AwayTeam.Text;

                lsFootball.Last().Bets = new Dictionary<string, double>();
                lsFootball.Last().Bets.Add("1", 0);
            }

            return lsFootball;
        }
    }
}
