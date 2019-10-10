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
            int i = 0;

            string date = DateTime.Now.ToString("dd.MM.yyyy");

            ChromeDriver browser = new ChromeDriver();

            List<Match> lsFootball = new List<Match>();

            browser.Navigate().GoToUrl($"https://sport.netbet.ro/fotbal/");

            Thread.Sleep(20000);

            ICollection<IWebElement> games = browser.FindElements(By.CssSelector("[class='rj-ev-list__content']"));

            browser.ExecuteScript("var p=window.XMLHttpRequest.prototype; p.open=p.send=p.setRequestHeader=function(){};");

            foreach (var item in games)
            {
                Console.WriteLine(i++);
                bool somethingwrong = false;

                int tries = 30;
                while (tries-- > 0)
                {
                    try
                    {
                        somethingwrong = false;
                        IWebElement HomeTeamMule = item.FindElement(By.CssSelector("[class='event-details-team-name event-details-team-a']"));
                        IWebElement AwayTeamMule = item.FindElement(By.CssSelector("[class='event-details-team-name event-details-team-a']"));
                        ICollection<IWebElement> oddsMule = item.FindElements(By.CssSelector("[class='bet-buttons-row ']"));
                    }
                    catch (Exception ex)
                    {
                        somethingwrong = true;
                        Console.WriteLine(ex.Message);
                    }

                    if (somethingwrong)
                    {
                        Thread.Sleep(10);
                        Console.WriteLine("Sleeping");
                    }
                    else
                    {
                        break;
                    }
                }

                if (tries == -1)
                {
                    break;
                }

                IWebElement HomeTeam = item.FindElement(By.CssSelector("[class='event-details-team-name event-details-team-a']"));
                IWebElement AwayTeam = item.FindElement(By.CssSelector("[class='event-details-team-name event-details-team-b']"));
                ICollection<IWebElement> odds = item.FindElements(By.CssSelector("[class='bet-odds-number']"));
                IWebElement time = item.FindElement(By.CssSelector("[data-uat='event-clock']"));

                lsFootball.Add(new Match());

                lsFootball.Last().HomeTeam = HomeTeam.GetAttribute("textContent");
                lsFootball.Last().AwayTeam = AwayTeam.GetAttribute("textContent");

                lsFootball.Last().Bets = new Dictionary<string, double>();

                int iterator = 0;

                foreach (var odd in odds)
                {
                    string bet = odd.GetAttribute("textContent");
                    if (bet.Contains(","))
                    {
                        bet.Replace(",", ".");
                    }

                    if (iterator == 0)
                    {
                        lsFootball.Last().Bets.Add("1", Double.Parse(bet));
                    }
                    if (iterator == 1)
                    {
                        lsFootball.Last().Bets.Add("X", Double.Parse(bet));
                    }
                    if (iterator == 2)
                    {
                        lsFootball.Last().Bets.Add("2", Double.Parse(bet));
                    }

                    iterator++;
                }
            }
            return lsFootball;
        }
    }
}
