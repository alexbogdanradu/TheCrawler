using System;
using System.Threading;


namespace TheCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(110, 30);
            Console.SetBufferSize(110, 10000);

            DateTime start = DateTime.Now;

            Ops operations = new Ops();

            /////////////////////////////////////////////////////////////////////////////////////////           1. GET MATCHES AND STANDINGS FROM WEB AND STORE THEM TO XML
            operations.GetMatchesAndStandingsFromWeb();

            /////////////////////////////////////////////////////////////////////////////////////////           1. GET PLAYED MATCHES FROM WEB AND STORE THEM TO XML
            operations.GetPlayedMatchesFromWeb();

            operations.browser.Dispose();

            DateTime stop = DateTime.Now;
            TimeSpan diff = stop - start;

            Console.WriteLine("");
            Console.WriteLine($"Process lasted {diff.Minutes} min, {diff.Seconds} sec. Extraction finished at {start.ToString()}");

            Thread.Sleep(5000);
        }

    }
}
