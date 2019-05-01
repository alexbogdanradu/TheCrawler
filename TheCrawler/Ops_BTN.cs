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
                string sHomeTeam;
                string sAwayTeam;
                string sDate;
                string sBet_1;
                string sBet_X;
                string sBet_2;
                string sOver25;
                string sUnder25;
                string sGG;
                string sNG;

                List<Match> localMatches = new List<Match>();

                string text;

                localMatches.Add(new Match());
                localMatches.Last().Bets = new Dictionary<string, double>();

                IWebElement WEMatchInfo = game.FindElement(By.ClassName("a94"));
                ICollection<IWebElement> WE1andX = game.FindElements(By.CssSelector("[class='alb ald']"));
                IWebElement WE2 = game.FindElement(By.ClassName("alb"));
                ICollection<IWebElement> WEPNandGG = game.FindElements(By.ClassName("a97"));

                IWebElement WSPlayingTeams = WEMatchInfo.FindElement(By.CssSelector("[class='js-event-click a1k']"));
                IWebElement WSMatchDate = WEMatchInfo.FindElement(By.ClassName("a0t"));

                text = WSPlayingTeams.Text;
                sHomeTeam = text.Substring(0, text.IndexOf("-") - 1);
                text = text.Substring(text.IndexOf("-") + 2, text.Length - text.IndexOf("-") - 3);
                sAwayTeam = text;

                sDate = WSMatchDate.Text;

                int i = 0;
                foreach (var item in WE1andX)
                {
                    if (i++ == 0)
                    {
                        sBet_1 = item.Text;
                        if (sBet_1.Contains(","))
                        {
                            sBet_1.Replace(",", ".");
                        }
                        localMatches.Last().Bets.Add("1", Double.Parse(sBet_1));
                    }
                    else
                    {
                        sBet_X = item.Text;
                        sBet_X = item.Text;
                        if (sBet_X.Contains(","))
                        {
                            sBet_X.Replace(",", ".");
                        }
                        localMatches.Last().Bets.Add("X", Double.Parse(sBet_X));
                    }
                }

                sBet_2 = WE2.Text;

                if (sBet_2.Contains(","))
                {
                    sBet_2.Replace(",", ".");
                }
                localMatches.Last().Bets.Add("2", Double.Parse(sBet_2));

                i = 0;
                foreach (var item in WEPNandGG)
                {
                    if (i++ == 0)
                    {
                        //Bets for PN
                        if (item.Text != "")
                        {
                            sOver25 = item.Text.Substring(item.Text.IndexOf("P ") + 2, item.Text.Length - item.Text.IndexOf("\r\n") - 4);
                            sUnder25 = item.Text.Substring(item.Text.IndexOf("S ") + 2, item.Text.Length - item.Text.IndexOf("S ") - 2);

                            if (sOver25.Contains(","))
                            {
                                sOver25.Replace(",", ".");
                            }
                            localMatches.Last().Bets.Add("O25", Double.Parse(sOver25));

                            if (sUnder25.Contains(","))
                            {
                                sUnder25.Replace(",", ".");
                            }
                            localMatches.Last().Bets.Add("U25", Double.Parse(sUnder25));
                        }
                    }
                    else
                    {
                        //Bets for GG NGG
                        if (item.Text != "")
                        {
                            sGG = item.Text.Substring(item.Text.IndexOf("Da ") + 3, item.Text.Length - item.Text.IndexOf("\r\n") - 5); 
                            sNG = item.Text.Substring(item.Text.IndexOf("Nu ") + 3, item.Text.Length - item.Text.IndexOf("\r\n") - 5);

                            if (sGG.Contains(","))
                            {
                                sGG.Replace(",", ".");
                            }
                            localMatches.Last().Bets.Add("GG", Double.Parse(sGG));

                            if (sNG.Contains(","))
                            {
                                sNG.Replace(",", ".");
                            }
                            localMatches.Last().Bets.Add("NG", Double.Parse(sNG));
                        }
                    }
                }

                localMatches.Last().HomeTeam = sHomeTeam;
                localMatches.Last().AwayTeam = sAwayTeam;
                localMatches.Last().PlayingDate = DateTime.ParseExact(sDate, "dd.MM HH:mm", null);

                lsFootball.AddRange(localMatches);
            }

            browser.Dispose();

            return lsFootball;
        }
    }
}
