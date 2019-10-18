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
using System.Threading.Tasks;

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

            Dictionary<string, string> leagues = new Dictionary<string, string>();
            //leagues.Add("anglia/premier-league", "https://www.flashscore.ro/fotbal/anglia/premier-league/arhiva/");
            //leagues.Add("franta/ligue-1", "https://www.flashscore.ro/fotbal/franta/ligue-1/arhiva/");
            //leagues.Add("germania/bundesliga", "https://www.flashscore.ro/fotbal/germania/bundesliga/arhiva/");
            //leagues.Add("italia/serie-a", "https://www.flashscore.ro/fotbal/italia/serie-a/arhiva/");
            //leagues.Add("romania/liga-1", "https://www.flashscore.ro/fotbal/romania/liga-1/arhiva/");
            //leagues.Add("spania/laliga", "https://www.flashscore.ro/fotbal/spania/laliga/arhiva/");
            leagues.Add("anglia/championship", "https://www.flashscore.ro/fotbal/anglia/championship/arhiva/");
            leagues.Add("anglia/league-one", "https://www.flashscore.ro/fotbal/anglia/league-one/arhiva/");
            leagues.Add("anglia/league-two", "https://www.flashscore.ro/fotbal/anglia/league-two/arhiva/");
            leagues.Add("anglia/national-league", "https://www.flashscore.ro/fotbal/anglia/national-league/arhiva/");
            leagues.Add("belgia/proximus-league", "https://www.flashscore.ro/fotbal/belgia/proximus-league/arhiva/");
            leagues.Add("belgia/first-amateur-division", "https://www.flashscore.ro/fotbal/belgia/first-amateur-division/arhiva/");
            leagues.Add("italia/serie-b", "https://www.flashscore.ro/fotbal/belgia/italia/serie-b/arhiva/");
            leagues.Add("italia/serie-c-group-a", "https://www.flashscore.ro/fotbal/italia/serie-c-group-a/arhiva/");
            leagues.Add("italia/serie-c-group-b", "https://www.flashscore.ro/fotbal/italia/serie-c-group-b/arhiva/");
            leagues.Add("italia/serie-c-group-c", "https://www.flashscore.ro/fotbal/italia/serie-c-group-c/arhiva/");
            leagues.Add("franta/ligue-2", "https://www.flashscore.ro/fotbal/franta/ligue-2/arhiva/");
            leagues.Add("franta/national", "https://www.flashscore.ro/fotbal/franta/national/arhiva/");
            leagues.Add("germania/2-bundesliga", "https://www.flashscore.ro/fotbal/germania/2-bundesliga/arhiva/");
            leagues.Add("germania/3-liga", "https://www.flashscore.ro/fotbal/germania/3-liga/arhiva/");


            List <ArchiveMatch> MasterArchive = new List<ArchiveMatch>();

            Dictionary<string, List<string>> leaguesArchive = new Dictionary<string, List<string>>();

            foreach (var item in leagues)
            {
                leaguesArchive.Add(item.Key, ops.FetchMatches_FlashScoreGetArchiveList(item.Value));
            }

            //int freq = 5; //How many at once
            //int counter = 0; //Should be 0
            //int duration = 120; //How much it should wait in seconds

            //foreach (var league in leaguesArchive)
            //{
            //    foreach (var archiveLink in league.Value)
            //    {
            //        counter++;
            //        if (counter % freq == 0)
            //        {
            //            Thread.Sleep(duration * 1000);
            //        }
            //        ops.FetchMatches_FlashScoreAsync(archiveLink);

            //    }
            //}

            int noOfConcurrentTasks = 5;

            List<Task> tasks = new List<Task>();

            foreach (var league in leaguesArchive)
            {
                tasks.AddRange(ops.GetListOfTasks(league.Value));
            }

            while (true)
            {
                Thread.Sleep(100);
                if (tasks.Where(o => o.Status == TaskStatus.RanToCompletion).Count() == tasks.Count)
                {
                    break;  
                }
                else
                {
                    Console.WriteLine($"{tasks.Where(o => o.Status == TaskStatus.RanToCompletion).Count()}/{tasks.Count} tasks completed. {tasks.Where(o => o.Status == TaskStatus.Running).Count()} tasks running.");
                    if (tasks.Where(o => o.Status == TaskStatus.Running).Count() < noOfConcurrentTasks)
                    {
                        if (tasks.Where(o => o.Status != TaskStatus.Running && o.Status != TaskStatus.RanToCompletion).Count() != 0)
                        {
                            try
                            {
                                tasks.Where(o => o.Status != TaskStatus.Running && o.Status != TaskStatus.RanToCompletion).First().Start();
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                }
            }

            //foreach (var league in leaguesArchive)
            //{
            //    foreach (var archiveLink in league.Value)
            //    {
            //        ops.FetchMatches_FlashScore(archiveLink);
            //    }
            //}

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

            //List<List<Match>> masterList = new List<List<Match>>();

            //masterList.Add(matches_sb);
            //masterList.Add(matches_nb);
            //masterList.Add(matches_cp);
            //masterList.Add(matches_btn);

            //string json = JsonConvert.SerializeObject(masterList);

            //using (StreamWriter sw = new StreamWriter("results.json"))
            //{
            //    sw.Write(json);
            //    sw.Flush();
            //    sw.Close();
            //}

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
