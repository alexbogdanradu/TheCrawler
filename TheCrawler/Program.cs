using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace TheCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(110, 30);
            Console.SetBufferSize(110, 10000);
            DateTime start = DateTime.Now;
            string timestamp = start.ToShortDateString().Replace(".", "");
            timestamp = timestamp.Replace("/", "");

            Ops op1 = new Ops();
            //op1.browser = new ChromeDriver();

            /////////////////////////////////////////////////////////////////////////////////////////           1. GET MATCHES AND STANDINGS FROM WEB AND STORE THEM TO XML
            //op1.InitAllLeagues();
            //Console.WriteLine($"Started at {start.ToShortTimeString().ToString()}");
            //Console.WriteLine("RUNNING IN HEADLESS MODE..");
            //op1.GetLatestMatchesAndStandingsFromWeb(ref op1.browser);
            //op1.SerializeToFile($"flashscoredb_{op1.lglist.Count}_leagues_{timestamp}.xml");
            //op1.Clean();

            /////////////////////////////////////////////////////////////////////////////////////////           2. GET MATCHES AND STANDINGS FROM THE FILE
            op1.GetLatestMatchesAndStandingsFromFile(@"flashscoredb_44_leagues_1272019.xml");

            /////////////////////////////////////////////////////////////////////////////////////////           3. ESTIMATE THE PROGNOSTIC FOR FUTURE GAMES
            List<Match> res1 = op1.DetermineBetabilityForFutureMatches();

            /////////////////////////////////////////////////////////////////////////////////////////           4. SHOW MATCHES TO BET
            op1.ShowFutureWinningMatchesByDaysFromNow(1, res1); //-1 for all, 3 for 3 days

            /////////////////////////////////////////////////////////////////////////////////////////           1. GET PLAYED MATCHES FROM WEB AND STORE THEM TO XML
            //op1.InitAllLeagues();
            //op1.GetPlayedMatchesFromWeb(ref op1.browser);
            //op1.SerializePlayedMatchesToFile($"flashscoredb_{op1.lglist.Count}_playedmatches_{DateTime.Now.ToShortDateString().Replace(".", "")}.xml");
            //op1.Clean();

            /////////////////////////////////////////////////////////////////////////////////////////           2. GET PLAYED MATCHES FROM THE FILE
            //op1.GetPlayedMatchesFromFile("flashscoredb_37_playedmatches_23012019.xml");

            /////////////////////////////////////////////////////////////////////////////////////////           3. ESTIMATE THE PROGNOSTIC FOR PAST GAMES
            //List<Match> res2 = op1.DetermineBetabilityForPastMatches(op1.lglist);

            DateTime stop = DateTime.Now;
            TimeSpan diff = stop - start;

            Console.WriteLine("");
            Console.WriteLine($"Extraction lasted {diff.Minutes} min, {diff.Seconds} sec. Extraction finished at {start.ToString()}");

            Console.Read();
        }

    }
}
