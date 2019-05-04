using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TheCrawler
{
    public class Ops
    {
        public List<CPMatch> LsMatches { get; set; }
        public List<CPMatch> LsBet1 { get; set; }

        public Ops()
        {
            LsMatches = new List<CPMatch>();
            LsBet1 = new List<CPMatch>();
        }

        public List<CPMatch> FetchMatches()
        {
            string date = DateTime.Now.ToString("dd.MM.yyyy");

            ChromeDriver browser = new ChromeDriver();

            List<CPMatch> lsFootball = new List<CPMatch>();

            browser.Navigate().GoToUrl($"https://www.casapariurilor.ro/Sports/offer?date={date}.");

            Thread.Sleep(5000);

            browser.FindElement(By.CssSelector("#retail-modal > div > div > div > button")).Click();

#if DEBUG

#else
            for (int i = 0; i < 120; i++)
            {
                Thread.Sleep(1000);
                ((IJavaScriptExecutor)browser).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 150)");
            }
#endif


            IWebElement table = browser.FindElement(By.ClassName("betting-tables"));

            ICollection<IWebElement> soccer = table.FindElements(By.ClassName("sport-group-type-soccer"));

            Console.WriteLine(soccer.Count + " matches found");

            int matchNo = 0;

            foreach (var item in soccer)
            {
                Console.WriteLine($"Loading match: {matchNo++}");

                Stopwatch sw = new Stopwatch();

                sw.Start();

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

                sw.Stop();

                Console.WriteLine($"Time spent while retrieving elements: {sw.ElapsedMilliseconds}, {homeTeams.Count} matches in this group.");

                Stopwatch sw2 = new Stopwatch();

                sw.Start();

                List<CPMatch> localMatches = new List<CPMatch>();

                if (homeTeams.Count == 0)
                {
                    break;
                }

                for (int i = 0; i < homeTeams.Count; i++)
                {
                    localMatches.Add(new CPMatch());
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

                foreach (var _date in dates)
                {
                    string time = _date.Text;
                    time = time.Substring(time.IndexOf(" ") + 1, time.Length - time.IndexOf(" ") - 1);

                    DateTime matchTime = DateTime.ParseExact(time, "HH:mm", null);
                    if (matchTime < DateTime.Now)
                    {
                        matchTime = matchTime.AddDays(1);
                    }

                    localMatches[iterator].PlayingDate = matchTime;
                    iterator++;
                }

                iterator = 0;

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
                                localMatches[i].Cote.Add(0);
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

                sw2.Stop();

                Console.WriteLine($"Processing took: {sw2.ElapsedMilliseconds}.");
            }

            browser.Dispose();

            return lsFootball;
        }

        public List<CPMatch> FindMatchesByAlgo(List<CPMatch> matches)
        {
            List<CPMatch> filteredList = new List<CPMatch>();

            foreach (var item in matches)
            {
                if (item.Bets["1"] != 0 && 
                    item.Bets["X"] != 0 &&
                    item.Bets["2"] != 0)
                {
                    filteredList.Add(item);
                }
            }

            foreach (var item in filteredList)
            {
                item.Bet01_Diff12 = Math.Abs(item.Bets["1"] - item.Bets["2"]);
                item.Bet01_DiffX1 = Math.Abs(item.Bets["X"] - item.Bets["1"]);
                item.Bet01_Diff2X = Math.Abs(item.Bets["2"] - item.Bets["X"]);

                item.MatchPercent = ((1 / item.Bets["1"]) + (1 / item.Bets["X"]) + (1 / item.Bets["2"])) * 100;

                //item.BetMin_Key = item.Bets.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                //item.BetMin_Val = item.Bets.Aggregate((l, r) => l.Value < r.Value ? l : r).Value;

                //item.Bet01_Diff12 = Math.Abs(item.Bets["1"] - item.Bets["2"]);
                //item.Bet01_DiffX1 = Math.Abs(item.Bets["X"] - item.Bets["1"]);
                //item.Bet01_Diff2X = Math.Abs(item.Bets["2"] - item.Bets["X"]);
            }

            LsBet1.AddRange(matches.OrderBy(o => o.Bet01_Diff12).Reverse().ToList());

            return LsBet1;
        }

        public string PrepareBody(List<CPMatch> matches)
        {
            string content = "";
            content = "Meciurile zilei:" + Environment.NewLine;
            content += Environment.NewLine;
            content += "Total meciuri: ";
            content += matches.Count;
            content += Environment.NewLine;
            content += Environment.NewLine;

            int iteration = 1;

            foreach (var match in matches)
            {
                content += iteration++.ToString();
                content += ". ";
                content += match.HomeTeam;
                content += " vs ";
                content += match.AwayTeam;
                content += Environment.NewLine;
content += match.PlayingDate.ToShortTimeString();
                content += Environment.NewLine;
                content += "Procent: ";
                content += match.MatchPercent;
                content += "%.";
                content += Environment.NewLine;
                content += Environment.NewLine;
                content += "1 vs 2 = ";
                content += match.Bet01_Diff12;
                content += Environment.NewLine;
                content += "X vs 1 = ";
                content += match.Bet01_DiffX1;
                content += Environment.NewLine;
                content += "2 vs X = ";
                content += match.Bet01_Diff2X;
                content += Environment.NewLine;
                content += "------------------";
                content += Environment.NewLine;

                foreach (var Bets in match.Bets)
                {
                    content += Bets.Key;
                    content += " -> ";
                    content += Bets.Value;
                    content += Environment.NewLine;
                }
                content += "------------------";
                content += Environment.NewLine;
                content += Environment.NewLine;
            }

            return content;
        }

        public void SendMail(string content)
        {
            SmtpClient client = new SmtpClient
            {
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Timeout = 10000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("oiganbet@gmail.com", "OiganBet1234+")
            };

            List<MailMessage> mails = new List<MailMessage>
            {
                new MailMessage("oiganbet@gmail.com", "alex_radu@live.com", "Daily Report", content)
            };

            foreach (var mail in mails)
            {
                client.Send(mail);
            }
        }
    }
}
