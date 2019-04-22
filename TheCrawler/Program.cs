using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
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

            string date = start.Date.ToString();

            ChromeDriver browser = new ChromeDriver();
            //ChromeOptions options;

            browser.Navigate().GoToUrl($"https://www.casapariurilor.ro/Sports/offer?date={date}.");

            IWebElement table = browser.FindElement(By.TagName("tbody"));

            ICollection<IWebElement> home = table.FindElements(By.ClassName("padl"));
            ICollection<IWebElement> away = table.FindElements(By.ClassName("padr"));
            ICollection<IWebElement> score = table.FindElements(By.ClassName("score"));
            ICollection<IWebElement> time = table.FindElements(By.ClassName("time"));

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
