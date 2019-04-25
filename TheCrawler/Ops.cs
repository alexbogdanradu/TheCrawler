using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TheCrawler
{
    public class Ops
    {
        public List<Match> lsMatches { get; set; }
        public List<Match> lsBet1 { get; set; }

        public Ops()
        {
            lsMatches = new List<Match>();
            lsBet1 = new List<Match>();
        }

        public List<Match> FetchMatches()
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

                    localMatches[i].Dict = new Dictionary<string, double>();

                    for (int j = 0; j < localMatches[i].Cote.Count; j++)
                    {
                        switch (j)
                        {
                            case 0:
                                localMatches[i].Dict.Add("1", localMatches[i].Cote[j]);
                                break;
                            case 1:
                                localMatches[i].Dict.Add("X", localMatches[i].Cote[j]);
                                break;
                            case 2:
                                localMatches[i].Dict.Add("2", localMatches[i].Cote[j]);
                                break;
                            case 3:
                                localMatches[i].Dict.Add("1X", localMatches[i].Cote[j]);
                                break;
                            case 4:
                                localMatches[i].Dict.Add("X2", localMatches[i].Cote[j]);
                                break;
                            case 5:
                                localMatches[i].Dict.Add("12", localMatches[i].Cote[j]);
                                break;
                            case 6:
                                localMatches[i].Dict.Add("F2", localMatches[i].Cote[j]);
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

        public List<Match> FindMatchesByAlgo(List<Match> matches)
        {
            foreach (var item in matches)
            {
                item.Bet01_Key = item.Dict.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                item.Bet01_Val = item.Dict.Aggregate((l, r) => l.Value < r.Value ? l : r).Value;

                item.Bet01_Diff12 = item.Dict["1"] - item.Dict["2"];
                item.Bet01_DiffX1 = item.Dict["X"] - item.Dict["1"];
                item.Bet01_Diff2X = item.Dict["2"] - item.Dict["X"];
            }

            lsBet1.AddRange(matches.OrderBy(o => o.Bet01_Val).ToList());

            return lsBet1;
        }

        public string PrepareBody(List<Match> matches)
        {
            string content = "";
            content = "Meciurile zilei:" + Environment.NewLine;
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
                content += "Cota minima: ";
                content += match.Bet01_Key;
                content += ", ";
                content += match.Bet01_Val;
                content += Environment.NewLine;

                content += "1 - 2 = ";
                content += match.Bet01_Diff12;
                content += Environment.NewLine;
                content += "X - 1 = ";
                content += match.Bet01_DiffX1;
                content += Environment.NewLine;
                content += "2 - X = ";
                content += match.Bet01_Diff2X;
                content += Environment.NewLine;

                content += "------------------";
                content += Environment.NewLine;

                foreach (var dict in match.Dict)
                {
                    content += dict.Key;
                    content += " -> ";
                    content += dict.Value;
                    content += Environment.NewLine;
                }

                content += Environment.NewLine;
            }

            return content;
        }

        public void SendMail(string content)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("oiganbet@gmail.com", "OiganBet1234+");

            List<MailMessage> mails = new List<MailMessage>();
            mails.Add(new MailMessage("oiganbet@gmail.com", "alex_radu@live.com", "Daily Report", content));
            mails.Add(new MailMessage("oiganbet@gmail.com", "alex.bogdan.radu@gmail.com", "Daily Report", content));

            foreach (var mail in mails)
            {
                client.Send(mail);
            }
        }
    }
}
