using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

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

            //List<CPMatch> matches = ops.FetchMatches();

            List<CPMatch> matches = new List<CPMatch>();
            using (StreamReader sr = new StreamReader("todaysmatches.json"))
            {
                matches = JsonConvert.DeserializeObject<List<CPMatch>>(sr.ReadToEnd());
            }

            List<CPMatch> foundMatches = ops.FindMatchesByAlgo(matches);

            //string json = JsonConvert.SerializeObject(matches);

            //using (StreamWriter sw = new StreamWriter("todaysmatches.json"))
            //{
            //    sw.Write(json);
            //}

            //string response = ops.PrepareBody(foundMatches);
            //ops.SendMail(response);

            DateTime stop = DateTime.Now;
            TimeSpan diff = stop - start;

            Console.WriteLine("");
            Console.WriteLine($"Extraction lasted {diff.Minutes} min, {diff.Seconds} sec. Extraction finished at {start.ToString()}");

            Thread.Sleep(5000);
            Console.Read();
        }

    }
}
