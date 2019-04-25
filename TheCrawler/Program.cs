﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace TheCrawler
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(110, 30);
            Console.SetBufferSize(110, 10000);
            DateTime start = DateTime.Now;

            string date = start.Date.ToString("dd.MM.yyyy");

            Ops ops = new Ops();

            List<Match> matches = ops.FetchMatches();
            List<Match> foundMatches = ops.FindMatchesByAlgo(matches);
            string response = ops.PrepareBody(foundMatches);
            ops.SendMail(response);

            //ops.SendMail(ops.PrepareBody(ops.FindMatchesByAlgo(ops.FetchMatches())));

            //ChromeDriver browser = new ChromeDriver();

            //List<Match> lsFootball = new List<Match>();

            //browser.Navigate().GoToUrl($"https://www.casapariurilor.ro/Sports/offer?date={date}.");

            //Thread.Sleep(5000);

            //browser.FindElement(By.CssSelector("#retail-modal > div > div > div > button")).Click();

            //for (int i = 0; i < 20; i++)
            //{
            //    Thread.Sleep(3000);
            //    ((IJavaScriptExecutor)browser).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 150)");
            //}

            //IWebElement table = browser.FindElement(By.ClassName("betting-tables"));

            //ICollection<IWebElement> soccer = table.FindElements(By.ClassName("sport-group-type-soccer"));

            //foreach (var item in soccer)
            //{
            //    ICollection<IWebElement> containers = table.FindElements(By.ClassName("sport-group-list-container"));

            //    List<Match> localMatches = new List<Match>();

            //    foreach (var container in containers)
            //    {
            //        ICollection<IWebElement> bets_type_1 = item.FindElements(By.CssSelector("div[data-pick='1']"));
            //        ICollection<IWebElement> bets_type_X = item.FindElements(By.CssSelector("div[data-pick='X']"));
            //        ICollection<IWebElement> bets_type_2 = item.FindElements(By.CssSelector("div[data-pick='2']"));
            //        ICollection<IWebElement> bets_type_1X = item.FindElements(By.CssSelector("div[data-pick='1X']"));
            //        ICollection<IWebElement> bets_type_X2 = item.FindElements(By.CssSelector("div[data-pick='X2']"));
            //        ICollection<IWebElement> bets_type_12 = item.FindElements(By.CssSelector("div[data-pick='12']"));
            //        ICollection<IWebElement> bets_type_F2 = item.FindElements(By.CssSelector("div[data-pick='F2']"));
            //        ICollection<IWebElement> homeTeams = item.FindElements(By.CssSelector("[class='event-header-team top']"));
            //        ICollection<IWebElement> awayTeams = item.FindElements(By.CssSelector("[class='event-header-team bottom']"));

            //        if (homeTeams.Count == 0 || awayTeams.Count == 0)
            //        {
            //            break;
            //        }

            //        for (int i = 0; i < homeTeams.Count; i++)
            //        {
            //            localMatches.Add(new Match());
            //        }

            //        int iterator = 0;

            //        foreach (var homeTeam in homeTeams)
            //        {
            //            localMatches[iterator].HomeTeam = homeTeam.Text;
            //            iterator++;
            //        }

            //        iterator = 0;

            //        foreach (var awayTeam in awayTeams)
            //        {
            //            localMatches[iterator].AwayTeam = awayTeam.Text;
            //            iterator++;
            //        }

            //        iterator = 0;

            //        foreach (var bet_type_1 in bets_type_1)
            //        {
            //            if (iterator == 7)
            //            {
            //                break;
            //            }

            //            if (localMatches[iterator].Bets == null)
            //            {
            //                localMatches[iterator].Bets = new Dictionary<string, double>();
            //            }

            //            if (bet_type_1.Text == "")
            //            {
            //                localMatches[iterator].Bets.Add("1", 999);
            //            }
            //            else
            //            {
            //                if (bet_type_1.Text.Contains(","))
            //                {
            //                    localMatches[iterator].Bets.Add("1", double.Parse(bet_type_1.Text.Replace(",", ".")));
            //                }
            //                else
            //                {
            //                    localMatches[iterator].Bets.Add("1", double.Parse(bet_type_1.Text));
            //                }
            //            }

            //            iterator++;
            //        }

            //        iterator = 0;

            //        foreach (var bet_type_X in bets_type_X)
            //        {
            //            if (iterator == 7)
            //            {
            //                break;
            //            }

            //            if (localMatches[iterator].Bets == null)
            //            {
            //                localMatches[iterator].Bets = new Dictionary<string, double>();
            //            }

            //            if (bet_type_X.Text == "")
            //            {
            //                localMatches[iterator].Bets.Add("X", 999);
            //            }
            //            else
            //            {
            //                if (bet_type_X.Text.Contains(","))
            //                {
            //                    localMatches[iterator].Bets.Add("X", double.Parse(bet_type_X.Text.Replace(",", ".")));
            //                }
            //                else
            //                {
            //                    localMatches[iterator].Bets.Add("X", double.Parse(bet_type_X.Text));
            //                }
            //            }

            //            iterator++;
            //        }

            //        iterator = 0;

            //        foreach (var bet_type_2 in bets_type_2)
            //        {
            //            if (iterator == 7)
            //            {
            //                break;
            //            }

            //            if (localMatches[iterator].Bets == null)
            //            {
            //                localMatches[iterator].Bets = new Dictionary<string, double>();
            //            }

            //            if (bet_type_2.Text == "")
            //            {
            //                localMatches[iterator].Bets.Add("2", 999);
            //            }
            //            else
            //            {
            //                if (bet_type_2.Text.Contains(","))
            //                {
            //                    localMatches[iterator].Bets.Add("2", double.Parse(bet_type_2.Text.Replace(",", ".")));
            //                }
            //                else
            //                {
            //                    localMatches[iterator].Bets.Add("2", double.Parse(bet_type_2.Text));
            //                }
            //            }

            //            iterator++;
            //        }

            //        iterator = 0;

            //        foreach (var bet_type_1X in bets_type_1X)
            //        {
            //            if (iterator == 7)
            //            {
            //                break;
            //            }

            //            if (localMatches[iterator].Bets == null)
            //            {
            //                localMatches[iterator].Bets = new Dictionary<string, double>();
            //            }

            //            if (bet_type_1X.Text == "")
            //            {
            //                localMatches[iterator].Bets.Add("1X", 999);
            //            }
            //            else
            //            {
            //                if (bet_type_1X.Text.Contains(","))
            //                {
            //                    localMatches[iterator].Bets.Add("1X", double.Parse(bet_type_1X.Text.Replace(",", ".")));
            //                }
            //                else
            //                {
            //                    localMatches[iterator].Bets.Add("1X", double.Parse(bet_type_1X.Text));
            //                }
            //            }

            //            iterator++;
            //        }

            //        iterator = 0;

            //        foreach (var bet_type_X2 in bets_type_X2)
            //        {
            //            if (iterator == 7)
            //            {
            //                break;
            //            }

            //            if (localMatches[iterator].Bets == null)
            //            {
            //                localMatches[iterator].Bets = new Dictionary<string, double>();
            //            }

            //            if (bet_type_X2.Text == "")
            //            {
            //                localMatches[iterator].Bets.Add("X2", 999);
            //            }
            //            else
            //            {
            //                if (bet_type_X2.Text.Contains(","))
            //                {
            //                    localMatches[iterator].Bets.Add("X2", double.Parse(bet_type_X2.Text.Replace(",", ".")));
            //                }
            //                else
            //                {
            //                    localMatches[iterator].Bets.Add("X2", double.Parse(bet_type_X2.Text));
            //                }
            //            }

            //            iterator++;
            //        }

            //        iterator = 0;

            //        foreach (var bet_type_12 in bets_type_12)
            //        {
            //            if (iterator == 7)
            //            {
            //                break;
            //            }

            //            if (localMatches[iterator].Bets == null)
            //            {
            //                localMatches[iterator].Bets = new Dictionary<string, double>();
            //            }

            //            if (bet_type_12.Text == "")
            //            {
            //                localMatches[iterator].Bets.Add("12", 999);
            //            }
            //            else
            //            {
            //                if (bet_type_12.Text.Contains(","))
            //                {
            //                    localMatches[iterator].Bets.Add("12", double.Parse(bet_type_12.Text.Replace(",", ".")));
            //                }
            //                else
            //                {
            //                    localMatches[iterator].Bets.Add("12", double.Parse(bet_type_12.Text));
            //                }
            //            }

            //            iterator++;
            //        }

            //        iterator = 0;

            //        foreach (var bet_type_F2 in bets_type_F2)
            //        {
            //            if (iterator == 7)
            //            {
            //                break;
            //            }

            //            if (localMatches[iterator].Bets == null)
            //            {
            //                localMatches[iterator].Bets = new Dictionary<string, double>();
            //            }

            //            if (bet_type_F2.Text == "")
            //            {
            //                localMatches[iterator].Bets.Add("F2", 999);
            //            }
            //            else
            //            {
            //                if (bet_type_F2.Text.Contains(","))
            //                {
            //                    localMatches[iterator].Bets.Add("F2", double.Parse(bet_type_F2.Text.Replace(",", ".")));
            //                }
            //                else
            //                {
            //                    localMatches[iterator].Bets.Add("F2", double.Parse(bet_type_F2.Text));
            //                }
            //            }

            //            iterator++;
            //        }

            //        iterator = 0;
            //    }

            //    lsFootball.AddRange(localMatches);
            //}



            //foreach (var item in soccer)
            //{
            //    ICollection<IWebElement> bets_type_1 = item.FindElements(By.CssSelector("div[data-pick='1']"));
            //    ICollection<IWebElement> bets_type_X = item.FindElements(By.CssSelector("div[data-pick='X']"));
            //    ICollection<IWebElement> bets_type_2 = item.FindElements(By.CssSelector("div[data-pick='2']"));
            //    ICollection<IWebElement> bets_type_1X = item.FindElements(By.CssSelector("div[data-pick='1X']"));
            //    ICollection<IWebElement> bets_type_X2 = item.FindElements(By.CssSelector("div[data-pick='X2']"));
            //    ICollection<IWebElement> bets_type_12 = item.FindElements(By.CssSelector("div[data-pick='12']"));
            //    ICollection<IWebElement> bets_type_F2 = item.FindElements(By.CssSelector("div[data-pick='F2']"));
            //    ICollection<IWebElement> homeTeams = item.FindElements(By.CssSelector("[class='event-header-team top']"));
            //    ICollection<IWebElement> awayTeams = item.FindElements(By.CssSelector("[class='event-header-team bottom']"));
            //    ICollection<IWebElement> dates = item.FindElements(By.CssSelector("[class='event-header-date-date']")); //plain text
            //    ICollection<IWebElement> cote = item.FindElements(By.CssSelector("[class='bet-pick bet-pick-3 ']")); //only first 5

            //    List<Match> localMatches = new List<Match>();

            //    if (homeTeams.Count == 0)
            //    {
            //        break;
            //    }

            //    for (int i = 0; i < homeTeams.Count; i++)
            //    {
            //        localMatches.Add(new Match());
            //    }

            //    int iterator = 0;

            //    foreach (var homeTeam in homeTeams)
            //    {
            //        localMatches[iterator].HomeTeam = homeTeam.Text;
            //        iterator++;
            //    }

            //    iterator = 0;

            //    foreach (var awayTeam in awayTeams)
            //    {
            //        localMatches[iterator].AwayTeam = awayTeam.Text;
            //        iterator++;
            //    }

            //    iterator = 0;

            //    //foreach (var _date in dates)
            //    //{
            //    //    localMatches[iterator].PlayingDate = _date.Text;
            //    //    iterator++;
            //    //}

            //    //iterator = 0;

            //    List<int> pointers = new List<int>();

            //    int divider = cote.Count / localMatches.Count;

            //    for (int i = 0; i < localMatches.Count; i++)
            //    {
            //        pointers.Add(divider * i);
            //    }

            //    int pointerUpper = 0;

            //    for (int i = 0; i < pointers.Count; i++)
            //    {
            //        localMatches[i].Cote = new List<double>();
            //        pointerUpper = pointers[i] + 7;

            //        foreach (var cota in cote)
            //        {
            //            if (iterator >= pointers[i] && iterator < pointerUpper)
            //            {
            //                if (cota.Text == "")
            //                {
            //                    localMatches[i].Cote.Add(20);
            //                }
            //                else
            //                {
            //                    if (cota.Text.Contains(","))
            //                    {
            //                        localMatches[i].Cote.Add(double.Parse(cota.Text.Replace(",", ".")));
            //                    }
            //                    else
            //                    {
            //                        localMatches[i].Cote.Add(double.Parse(cota.Text));
            //                    }
            //                }
            //            }
            //            iterator++;
            //        }

            //        localMatches[i].Dict = new Dictionary<string, double>();

            //        for (int j = 0; j < localMatches[i].Cote.Count; j++)
            //        {
            //            switch (j)
            //            {
            //                case 0:
            //                    localMatches[i].Dict.Add("1", localMatches[i].Cote[j]);
            //                    break;
            //                case 1:
            //                    localMatches[i].Dict.Add("X", localMatches[i].Cote[j]);
            //                    break;
            //                case 2:
            //                    localMatches[i].Dict.Add("2", localMatches[i].Cote[j]);
            //                    break;
            //                case 3:
            //                    localMatches[i].Dict.Add("1X", localMatches[i].Cote[j]);
            //                    break;
            //                case 4:
            //                    localMatches[i].Dict.Add("X2", localMatches[i].Cote[j]);
            //                    break;
            //                case 5:
            //                    localMatches[i].Dict.Add("12", localMatches[i].Cote[j]);
            //                    break;
            //                case 6:
            //                    localMatches[i].Dict.Add("F2", localMatches[i].Cote[j]);
            //                    break;
            //                default:
            //                    break;
            //            }
            //        }

            //        iterator = 0;
            //    }
            //    lsFootball.AddRange(localMatches);
            //}

            //foreach (var item in lsFootball)
            //{
            //    item.Bet01_Key = item.Dict.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            //    item.Bet01_Val = item.Dict.Aggregate((l, r) => l.Value < r.Value ? l : r).Value;
            //}

            //List<Match> orderedList = new List<Match>();
            //orderedList.AddRange(lsFootball.OrderBy(o => o.Bet01_Val).ToList());

            //string content = "";
            //content = "Meciurile zilei:" + Environment.NewLine;
            //content += Environment.NewLine;

            //int iteration = 1;

            //foreach (var item in orderedList)
            //{
            //    content += iteration++.ToString();
            //    content += ". ";
            //    content += item.HomeTeam;
            //    content += " vs ";
            //    content += item.AwayTeam;
            //    content += ". Cota minima: ";
            //    content += item.Bet01_Key;
            //    content += ", ";
            //    content += item.Bet01_Val;
            //    content += Environment.NewLine;
            //    content += Environment.NewLine;
            //}

            //SmtpClient client = new SmtpClient();
            //client.Port = 587;
            //client.Host = "smtp.gmail.com";
            //client.EnableSsl = true;
            //client.Timeout = 10000;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = false;
            //client.Credentials = new System.Net.NetworkCredential("oiganbet@gmail.com", "OiganBet1234+");

            //List<MailMessage> mails = new List<MailMessage>();
            //mails.Add(new MailMessage("oiganbet@gmail.com", "alex_radu@live.com", "Daily Report", content));
            //mails.Add(new MailMessage("oiganbet@gmail.com", "alex.bogdan.radu@gmail.com", "Daily Report", content));

            //foreach (var mail in mails)
            //{
            //    client.Send(mail);
            //}

            DateTime stop = DateTime.Now;
            TimeSpan diff = stop - start;

            //browser.Dispose();

            Console.WriteLine("");
            Console.WriteLine($"Extraction lasted {diff.Minutes} min, {diff.Seconds} sec. Extraction finished at {start.ToString()}");
        }

    }
}
