using System;
using System.Collections.Generic;


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

            List<CPMatch> matches = ops.FetchMatches();
            List<CPMatch> foundMatches = ops.FindMatchesByAlgo(matches);
            string response = ops.PrepareBody(foundMatches);
            ops.SendMail(response);

            DateTime stop = DateTime.Now;
            TimeSpan diff = stop - start;

            Console.WriteLine("");
            Console.WriteLine($"Extraction lasted {diff.Minutes} min, {diff.Seconds} sec. Extraction finished at {start.ToString()}");
        }

    }
}
