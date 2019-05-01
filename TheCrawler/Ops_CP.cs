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
        public List<Match> FetchMatches_CP()
        {
            string date = DateTime.Now.ToString("dd.MM.yyyy");

            ChromeDriver browser = new ChromeDriver();

            List<Match> lsFootball = new List<Match>();

            browser.Navigate().GoToUrl($"https://www.casapariurilor.ro/Sports/offer?date={date}.");

            Thread.Sleep(5000);

            browser.FindElement(By.CssSelector("#retail-modal > div > div > div > button")).Click();

#if DEBUG

#else
            for (int i = 0; i < 20; i++)
            {
                Thread.Sleep(3000);
                ((IJavaScriptExecutor)browser).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 150)");
            }
#endif


            IWebElement table = browser.FindElement(By.ClassName("betting-tables"));

            ICollection<IWebElement> soccer = table.FindElements(By.ClassName("sport-group-type-soccer"));

            foreach (var item in soccer)
            {
                ICollection<IWebElement> bets_type_1 = item.FindElements(By.CssSelector("div[data-pick='1']"));
                ICollection<IWebElement> bets_type_X = item.FindElements(By.CssSelector("div[data-pick='X']"));
                ICollection<IWebElement> bets_type_2 = item.FindElements(By.CssSelector("div[data-pick='2']"));
                ICollection<IWebElement> bets_type_1X = item.FindElements(By.CssSelector("div[data-pick='1X']"));
                ICollection<IWebElement> bets_type_X2 = item.FindElements(By.CssSelector("div[data-pick='X2']"));
                ICollection<IWebElement> bets_type_12 = item.FindElements(By.CssSelector("div[data-pick='12']"));
                ICollection<IWebElement> bets_type_F2 = item.FindElements(By.CssSelector("div[data-pick='F2']"));
                ICollection<IWebElement> homeTeams = item.FindElements(By.CssSelector("[class='event-header-team top']"));
                ICollection<IWebElement> awayTeams = item.FindElements(By.CssSelector("[class='event-header-team bottom']"));
                ICollection<IWebElement> dates = item.FindElements(By.CssSelector("[class='event-header-date-date']")); //plain text
                ICollection<IWebElement> cote = item.FindElements(By.CssSelector("[class='bet-pick bet-pick-3 ']")); //only first 5

                List<Match> localMatches = new List<Match>();

                if (homeTeams.Count == 0)
                {
                    break;
                }

                for (int i = 0; i < homeTeams.Count; i++)
                {
                    localMatches.Add(new Match());
                }

                int iterator = 0;

                foreach (var homeTeam in homeTeams)
                {
                    localMatches[iterator].HomeTeam = homeTeam.Text;
                    iterator++;
                }

                iterator = 0;

                foreach (var awayTeam in awayTeams)
                {
                    localMatches[iterator].AwayTeam = awayTeam.Text;
                    iterator++;
                }

                iterator = 0;

                //foreach (var _date in dates)
                //{
                //    localMatches[iterator].PlayingDate = _date.Text;
                //    iterator++;
                //}

                //iterator = 0;

                List<int> pointers = new List<int>();

                int divider = cote.Count / localMatches.Count;

                for (int i = 0; i < localMatches.Count; i++)
                {
                    pointers.Add(divider * i);
                }

                int pointerUpper = 0;

                for (int i = 0; i < pointers.Count; i++)
                {
                    localMatches[i].Cote = new List<double>();
                    pointerUpper = pointers[i] + 7;

                    foreach (var cota in cote)
                    {
                        if (iterator >= pointers[i] && iterator < pointerUpper)
                        {
                            if (cota.Text == "")
                            {
                                localMatches[i].Cote.Add(20);
                            }
                            else
                            {
                                if (cota.Text.Contains(","))
                                {
                                    localMatches[i].Cote.Add(double.Parse(cota.Text.Replace(",", ".")));
                                }
                                else
                                {
                                    localMatches[i].Cote.Add(double.Parse(cota.Text));
                                }
                            }
                        }
                        iterator++;
                    }

                    localMatches[i].Bets = new Dictionary<string, double>();

                    for (int j = 0; j < localMatches[i].Cote.Count; j++)
                    {
                        switch (j)
                        {
                            case 0:
                                localMatches[i].Bets.Add("1", localMatches[i].Cote[j]);
                                break;
                            case 1:
                                localMatches[i].Bets.Add("X", localMatches[i].Cote[j]);
                                break;
                            case 2:
                                localMatches[i].Bets.Add("2", localMatches[i].Cote[j]);
                                break;
                            case 3:
                                localMatches[i].Bets.Add("1X", localMatches[i].Cote[j]);
                                break;
                            case 4:
                                localMatches[i].Bets.Add("X2", localMatches[i].Cote[j]);
                                break;
                            case 5:
                                localMatches[i].Bets.Add("12", localMatches[i].Cote[j]);
                                break;
                            case 6:
                                localMatches[i].Bets.Add("F2", localMatches[i].Cote[j]);
                                break;
                            default:
                                break;
                        }
                    }

                    iterator = 0;
                }
                lsFootball.AddRange(localMatches);
            }

            browser.Dispose();

            return lsFootball;
        }

    }
}
