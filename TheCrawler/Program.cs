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
            Console.SetBufferSize(110, 30);
            DateTime start = DateTime.Now;
            Ops op1 = new Ops();
            //op1.browser = new ChromeDriver();

            /////////////////////////////////////////////////////////////////////////////////////////GET MATCHES AND STANDINGS FROM WEB AND STORE THEM TO XML
            //op1.InitAllLeagues();
            //op1.GetLatestMatchesAndStandingsFromWeb(ref op1.browser);
            //op1.SerializeToFile($"flashscoredb_{op1.lglist.Count}_leagues_{DateTime.Now.ToShortDateString().Replace(".", "")}.xml");
            //op1.Clean();

            /////////////////////////////////////////////////////////////////////////////////////////GET MATCHES AND STANDINGS FROM THE FILE
            op1.GetLatestMatchesAndStandingsFromFile(@"flashscoredb_33_leagues_25012019.xml");

            /////////////////////////////////////////////////////////////////////////////////////////GET PLAYED MATCHES FROM WEB AND STORE THEM TO XML
            //op1.InitAllLeagues();
            //op1.GetPlayedMatchesFromWeb(ref op1.browser);
            //op1.SerializePlayedMatchesToFile($"flashscoredb_{op1.lglist.Count}_playedmatches_{DateTime.Now.ToShortDateString().Replace(".", "")}.xml");

            /////////////////////////////////////////////////////////////////////////////////////////GET PLAYED MATCHES FROM THE FILE
            //op1.GetPlayedMatchesFromFile("flashscoredb_37_playedmatches_23012019.xml");

            /////////////////////////////////////////////////////////////////////////////////////////ESTIMATE THE PROGNOSTIC FOR FUTURE GAMES
            List<Match> res1 = op1.DetermineBetabilityForFutureMatches();

            /////////////////////////////////////////////////////////////////////////////////////////ESTIMATE THE PROGNOSTIC FOR PAST GAMES
            //List<Match> res2 = op1.DetermineBetabilityForPastMatches(op1.lglist);

            /////////////////////////////////////////////////////////////////////////////////////////CALCULATE ACTUAL SCORE
            //List<Match> res3 = op1.FillActualScore(res2);

            //int Success = 0;
            //int Fail = 0;

            //foreach (var item in res3)
            //{
            //    if (item.strResultActual == item.strResultEstimated)
            //    {
            //        Success++;
            //    }
            //    else
            //    {
            //        Fail++;
            //    }
            //}

            /////////////////////////////////////////////////////////////////////////////////////////SHOW MATCHES TO BET
            op1.ShowFutureWinningMatchesByDaysFromNow(3, res1); //-1 for all, 3 for 3 days
            
            DateTime stop = DateTime.Now;
            TimeSpan diff = stop - start;

            Console.WriteLine("");
            Console.WriteLine($"Extraction lasted {diff.Minutes} min, {diff.Seconds} sec. Extraction finished at {start.ToString()}");

            Console.Read();
        }

    }
}
