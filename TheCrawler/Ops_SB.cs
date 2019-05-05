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

            IWebElement cookiewindow = browser.FindElement(By.CssSelector("[aria-labelledby='ui-dialog-title-cookieMessage']"));
            cookiewindow.FindElement(By.CssSelector("[class='ui-icon ui-icon-closethick']")).Click();

            IWebElement window = browser.FindElement(By.CssSelector("[aria-labelledby='ui-dialog-title-age18Message']"));

            window.FindElement(By.CssSelector("[class='successButton ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only']")).Click();

            IWebElement games = browser.FindElement(By.CssSelector("[class='oddsTable drop-shadow lifted']"));

            ICollection<IWebElement> odds = games.FindElements(By.ClassName("trOdd"));
            ICollection<IWebElement> evens = games.FindElements(By.ClassName("trEven"));

            foreach (var item in odds)
            {
                lsFootball.Add(new Match());

                string teams = item.FindElement(By.CssSelector("[class='alignLeft fullWidth']")).GetAttribute("textContent");
                ICollection<IWebElement> tds = item.FindElements(By.TagName("td"));

                int iterator = 0;

                string home = teams.Substring(0, teams.Length - teams.IndexOf("-") - 3);
                string away = teams.Substring(teams.IndexOf("-") + 2);

                lsFootball.Last().HomeTeam = home;
                lsFootball.Last().AwayTeam = away;

                lsFootball.Last().Bets = new Dictionary<string, double>();

                foreach (var td in tds)
                {
                    if ( iterator == 1) //hour
                    {
                        lsFootball.Last().PlayingDate = DateTime.ParseExact(td.GetAttribute("textContent"), "HH:mm", null);
                    }

                    if (iterator == 2) //date
                    {
                        if (lsFootball.Last().PlayingDate.Hour != DateTime.Now.Hour)
                        {
                            lsFootball.Last().PlayingDate.AddDays(1);
                        }
                    }

                    if (iterator == 5) //1
                    {
                        string cota = td.GetAttribute("textContent");

                        if (cota == "-")
                        {
                            break;
                        }

                        if (cota.Contains(","))
                        {
                            cota.Replace(",", ".");
                        }

                        lsFootball.Last().Bets.Add("1", Double.Parse(cota));
                    }

                    if (iterator == 6) //X
                    {
                        string cota = td.GetAttribute("textContent");

                        if (cota == "-")
                        {
                            break;
                        }

                        if (cota.Contains(","))
                        {
                            cota.Replace(",", ".");
                        }

                        lsFootball.Last().Bets.Add("X", Double.Parse(cota));
                    }

                    if (iterator == 7) //2
                    {
                        string cota = td.GetAttribute("textContent");

                        if (cota == "-")
                        {
                            break;
                        }

                        if (cota.Contains(","))
                        {
                            cota.Replace(",", ".");
                        }

                        lsFootball.Last().Bets.Add("2", Double.Parse(cota));
                    }

                    iterator++;
                }
            }

            foreach (var item in evens)
            {
                lsFootball.Add(new Match());

                string teams = item.FindElement(By.CssSelector("[class='alignLeft fullWidth']")).GetAttribute("textContent");
                ICollection<IWebElement> tds = item.FindElements(By.TagName("td"));

                int iterator = 0;

                string home = teams.Substring(0, teams.Length - teams.IndexOf("-") - 3);
                string away = teams.Substring(teams.IndexOf("-") + 2);

                lsFootball.Last().HomeTeam = home;
                lsFootball.Last().AwayTeam = away;

                lsFootball.Last().Bets = new Dictionary<string, double>();

                foreach (var td in tds)
                {
                    if (iterator == 1) //hour
                    {
                        lsFootball.Last().PlayingDate = DateTime.ParseExact(td.GetAttribute("textContent"), "HH:mm", null);
                    }

                    if (iterator == 2) //date
                    {
                        if (lsFootball.Last().PlayingDate.Hour != DateTime.Now.Hour)
                        {
                            lsFootball.Last().PlayingDate.AddDays(1);
                        }
                    }

                    if (iterator == 5) //1
                    {
                        string cota = td.GetAttribute("textContent");

                        if (cota == "-")
                        {
                            break;
                        }

                        if (cota.Contains(","))
                        {
                            cota.Replace(",", ".");
                        }

                        lsFootball.Last().Bets.Add("1", Double.Parse(cota));
                    }

                    if (iterator == 6) //X
                    {
                        string cota = td.GetAttribute("textContent");

                        if (cota == "-")
                        {
                            break;
                        }

                        if (cota.Contains(","))
                        {
                            cota.Replace(",", ".");
                        }

                        lsFootball.Last().Bets.Add("X", Double.Parse(cota));
                    }

                    if (iterator == 7) //2
                    {
                        string cota = td.GetAttribute("textContent");

                        if (cota == "-")
                        {
                            break;
                        }

                        if (cota.Contains(","))
                        {
                            cota.Replace(",", ".");
                        }

                        lsFootball.Last().Bets.Add("2", Double.Parse(cota));
                    }

                    iterator++;
                }
            }

            browser.Dispose();

            return lsFootball;
        }
    }
}
