using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
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

            ChromeDriver browser = new ChromeDriver();
            //ChromeOptions options;

            List<Match> lsFootball = new List<Match>();

            browser.Navigate().GoToUrl($"https://www.casapariurilor.ro/Sports/offer?date={date}.");

            Thread.Sleep(5000);

            browser.FindElement(By.CssSelector("#retail-modal > div > div > div > button")).Click();

            //for (int i = 0; i < 10; i++)
            //{
            //    Thread.Sleep(2000);
            //    ((IJavaScriptExecutor)browser).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 150)");
            //}

            IWebElement table = browser.FindElement(By.ClassName("betting-tables"));

            ICollection<IWebElement> soccer = table.FindElements(By.ClassName("sport-group-type-soccer"));

            foreach (var item in soccer)
            {
                ICollection<IWebElement> homeTeams = item.FindElements(By.CssSelector("[class='event-header-team top']"));
                ICollection<IWebElement> awayTeams = item.FindElements(By.CssSelector("[class='event-header-team bottom']"));
                ICollection<IWebElement> dates = item.FindElements(By.CssSelector("[class='event-header-date-date']")); //plain text
                ICollection<IWebElement> cote = item.FindElements(By.CssSelector("[class='bet-pick bet-pick-3 ']")); //only first 5

                List<Match> localMatches = new List<Match>();

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

                foreach (var _date in dates)
                {
                    localMatches[iterator].PlayingDate = _date.Text;
                    iterator++;
                }

                iterator = 0;

                List<int> pointers = new List<int>();

                int divider = cote.Count / localMatches.Count;

                for (int i = 0; i < localMatches.Count; i++)
                {
                    pointers.Add(divider * i);
                }

                for (int i = 0; i < pointers.Count; i++)
                {
                    foreach (var cota in cote)
                    {
                        if (iterator == pointers[i])
                        {
                            localMatches[i].Cote.Add(double.Parse(cota.Text));
                        }
                        iterator++;
                    }
                    iterator = 0;
                }

                lsFootball.AddRange(localMatches);
            }

            string password;

            using (StreamReader sr = new StreamReader("password.txt"))
            {
                password = sr.ReadToEnd();
            }

            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("alex.bogdan.radu@gmail.com", password);

            MailMessage mm = new MailMessage("alex.bogdan.radu@gmail.com", "alex_radu@live.com", "test", "test");
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.Send(mm);

            DateTime stop = DateTime.Now;
            TimeSpan diff = stop - start;

            Console.WriteLine("");
            Console.WriteLine($"Extraction lasted {diff.Minutes} min, {diff.Seconds} sec. Extraction finished at {start.ToString()}");

            Console.Read();
        }

    }
}
