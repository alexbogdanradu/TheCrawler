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

            ICollection<IWebElement> games = browser.FindElements(By.ClassName("event-wrapper-inner"));

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
                    catch (Exception)
                    {
                        somethingwrong = true;
                    }

                    if (somethingwrong)
                    {
                        Thread.Sleep(10);
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
                ICollection<IWebElement> odds = item.FindElements(By.CssSelector("[class='bet-buttons-row ']"));

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

                Console.Write(AwayTeam.Text);

                lsFootball.Last().Bets = new Dictionary<string, double>();
                lsFootball.Last().Bets.Add("1", 0);
            }
            return lsFootball;
        }
    }
}
