using ArchiveModel.Models.Database;
using Newtonsoft.Json;
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
using Microsoft.EntityFrameworkCore;

namespace TheCrawler
{
    partial class Program
    {
        static void Main(string[] args)
        {
            //var context = new betsContext();

            //var t = new Archive
            //{   
            //    AwayTeam = "awt",
            //    HomeTeamScore = 2,
            //    AwayTeamScore = 1,
            //    HomeTeam = "hmt",
            //    League = "ka",
            //    PlayingDate = DateTime.Now
            //};

            //context.Archive.Add(t);
            //context.SaveChanges();

            Console.SetWindowSize(110, 30);
            Console.SetBufferSize(110, 10000);
            DateTime start = DateTime.Now;

            string date = start.Date.ToString("dd.MM.yyyy");

            Ops ops = new Ops();

            Dictionary<string, string> leagues = new Dictionary<string, string>();
            leagues.Add("PremiereLeague", "https://www.flashscore.ro/fotbal/anglia/premier-league/arhiva/");
            leagues.Add("Ligue1", "https://www.flashscore.ro/fotbal/franta/ligue-1/arhiva/");
            leagues.Add("Bundesliga", "https://www.flashscore.ro/fotbal/germania/bundesliga/arhiva/");
            leagues.Add("SerieA", "https://www.flashscore.ro/fotbal/italia/serie-a/arhiva/");
            leagues.Add("Liga1", "https://www.flashscore.ro/fotbal/romania/liga-1/arhiva/");
            leagues.Add("LaLiga", "https://www.flashscore.ro/fotbal/spania/laliga/arhiva/");

            List<ArchiveMatch> MasterArchive = new List<ArchiveMatch>();

            Dictionary<string, List<string>> leaguesArchive = new Dictionary<string, List<string>>();

            foreach (var item in leagues)
            {
                leaguesArchive.Add(item.Key, ops.FetchMatches_FlashScoreGetArchiveList(item.Value));
            }

            //int freq = 2;
            //int counter = 0;
            //int duration = 60;

            //foreach (var league in leaguesArchive)
            //{
            //    foreach (var archiveLink in league.Value)
            //    {
            //        counter++;
            //        if (counter%freq == 0)
            //        {
            //            Thread.Sleep(duration * 1000);
            //        }
            //        ops.FetchMatches_FlashScoreAsync(archiveLink);
            //    }
            //}

            foreach (var league in leaguesArchive)
            {
                foreach (var archiveLink in league.Value)
                {
                    //counter++;
                    //if (counter % freq == 0)
                    //{
                    //    Thread.Sleep(duration * 1000);
                    //}
                    ops.FetchMatches_FlashScore(archiveLink);
                    //ops.FetchMatches_FlashScoreAsync(archiveLink);
                }
            }

            //foreach (var league in leaguesArchive)
            //{
            //    foreach (var archiveLink in league.Value)
            //    {
            //        MasterArchive.AddRange(ops.FetchMatches_FlashScore(archiveLink));
            //        foreach (var item in MasterArchive)
            //        {
            //            Console.WriteLine($"{item.HomeTeam} vs {item.AwayTeam}");
            //        }
            //    }
            //}

            //List<Match> matches_sb = ops.FetchMatches_SB(); 
            //List<Match> matches_nb = ops.FetchMatches_NB();
            //List<Match> matches_cp = ops.FetchMatches_CP();
            //List<Match> matches_btn = ops.FetchMatches_BTN();

            List<List<Match>> masterList = new List<List<Match>>();

            //masterList.Add(matches_sb);
            //masterList.Add(matches_nb);
            //masterList.Add(matches_cp);
            //masterList.Add(matches_btn);

            string json = JsonConvert.SerializeObject(masterList);

            using (StreamWriter sw = new StreamWriter("results.json"))
            {
                sw.Write(json);
                sw.Flush();
                sw.Close();
            }

            //List<Match> foundMatches = ops.FindMatchesByAlgo(matches_cp);
            //string response = ops.PrepareBody(foundMatches);
            //ops.SendMail(json);

            DateTime stop = DateTime.Now;
            TimeSpan diff = stop - start;

            Console.WriteLine("");
            Console.WriteLine($"Extraction lasted {diff.Minutes} min, {diff.Seconds} sec. Extraction finished at {start.ToString()}");
            Console.Read();
        }

    }
}
