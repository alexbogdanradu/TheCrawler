using OpenQA.Selenium;
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

            //List<Match> matches_sb = ops.FetchMatches_SB(); 
            //List<Match> matches_nb = ops.FetchMatches_NB();
            //List<Match> matches_cp = ops.FetchMatches_CP();
            //List<Match> matches_btn = ops.FetchMatches_BTN();

            //List<Match> foundMatches = ops.FindMatchesByAlgo(matches_cp);
            //string response = ops.PrepareBody(foundMatches);
            //ops.SendMail(response);
            
            DateTime stop = DateTime.Now;
            TimeSpan diff = stop - start;

            Console.WriteLine("");
            Console.WriteLine($"Extraction lasted {diff.Minutes} min, {diff.Seconds} sec. Extraction finished at {start.ToString()}");
        }

    }
}
