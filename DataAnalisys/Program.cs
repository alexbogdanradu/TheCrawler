﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAnalisys
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ArchiveMatch> matches = new List<ArchiveMatch>();

            //get the matches from the file
            using (StreamReader sr = new StreamReader("allLeagues.json"))
            {
                matches.AddRange(JsonConvert.DeserializeObject<List<ArchiveMatch>>(sr.ReadToEnd()));
            }

            //Get all unique away teams
            List<string> teams = matches.Select(o => o.HomeTeam).Distinct().ToList();
            teams.AddRange(matches.Select(o => o.AwayTeam).Distinct().ToList());
            teams = teams.Distinct().ToList();

            //dictionary containing name of the team and its played matches
            Dictionary<string, List<ArchiveMatch>> matchesPlayedByAteam = new Dictionary<string, List<ArchiveMatch>>();
            List<Team> allTeams = new List<Team>();

            //get all the distinct teams
            foreach (var team in teams)
            {
                if (matchesPlayedByAteam.Keys.Contains(team))
                {
                    matchesPlayedByAteam[team].AddRange(matches.Where(o => o.HomeTeam == team || o.AwayTeam == team).OrderBy(o => o.PlayingDate).ToList());
                }
                else
                {
                    matchesPlayedByAteam.Add(team, matches.Where(o => o.HomeTeam == team || o.AwayTeam == team).OrderBy(o => o.PlayingDate).ToList());
                }
            }

            //generate pattern strings
            List<string> patterns = new List<string>();

            string theString = "111";

            for (int j = 0; j < theString.Length; j++)
            {
                for (int i = 0; i < theString.Length; i++)
                {
                    if (i == 0)
                    {
                        char[] arr = theString.ToCharArray();
                        arr[j] = '1';
                        theString = new string(arr);
                        patterns.Add(theString);
                    }
                    if (i == 1)
                    {
                        char[] arr = theString.ToCharArray();
                        arr[j] = 'x';
                        theString = new string(arr);
                        patterns.Add(theString);
                    }
                    if (i == 2)
                    {
                        char[] arr = theString.ToCharArray();
                        arr[j] = '2';
                        theString = new string(arr);
                        patterns.Add(theString);
                    }
                }
            }

            foreach (var teamMatches in matchesPlayedByAteam)
            {
                List<ArchiveMatch> archiveMatches = teamMatches.Value.OrderBy(o => o.PlayingDate).ToList();
                teamMatches.Value.RemoveAll(o => o.HomeTeam != null);
                teamMatches.Value.AddRange(archiveMatches);

                allTeams.Add(new Team { TeamName = teamMatches.Key });

                List<string> results = new List<string>();

                foreach (var item in teamMatches.Value)
                {

                    if (item.Result == "1" && item.HomeTeam == teamMatches.Key)
                    {
                        results.Add("1");
                    }
                    else if (item.Result == "2" && item.AwayTeam == teamMatches.Key)
                    {
                        results.Add("1");
                    }
                    else
                    {
                        results.Add("0");
                    }
                }

                for (int i = 0; i < results.Count; i++)
                {
                    if (results[i] == "1")
                    {
                        allTeams.Last().wins++;
                    }

                    if (i < results.Count - 1)
                    {
                        if (results[i] == "0" && results[i + 1] == "1")
                        {
                            allTeams.Last().pat01++;
                        }
                        if (results[i] == "0" && results[i + 1] == "0")
                        {
                            allTeams.Last().pat00++;
                        }
                        if (results[i] == "1" && results[i + 1] == "0")
                        {
                            allTeams.Last().pat10++;
                        }
                        if (results[i] == "1" && results[i + 1] == "1")
                        {
                            allTeams.Last().pat11++;
                        }
                    }

                    if (i < results.Count - 2)
                    {
                        if (results[i] == "1" && results[i + 1] == "1" && results[i + 2] == "1")
                        {
                            allTeams.Last().pat111++;
                        }
                        if (results[i] == "0" && results[i + 1] == "0" && results[i + 2] == "0")
                        {
                            allTeams.Last().pat000++;
                        }
                    }
                }
            }

            foreach (var team in allTeams)
            {
                Console.Clear();
                Console.WriteLine($"Team:\t\t{team.TeamName}");
                Console.WriteLine($"Games played:\t{team.plays}");
                Console.WriteLine($"00:\t\t{team.pat00}.\tMax: {allTeams.Max(o => o.pat00)}.\tAvg: {allTeams.Average(o => o.pat00).ToString().Substring(0, 6)}");
                Console.WriteLine($"01:\t\t{team.pat01}.\tMax: {allTeams.Max(o => o.pat01)}.\tAvg: {allTeams.Average(o => o.pat01).ToString().Substring(0, 6)}");
                Console.WriteLine($"10:\t\t{team.pat10}.\tMax: {allTeams.Max(o => o.pat10)}.\tAvg: {allTeams.Average(o => o.pat10).ToString().Substring(0, 6)}");
                Console.WriteLine($"11:\t\t{team.pat11}.\tMax: {allTeams.Max(o => o.pat11)}.\tAvg: {allTeams.Average(o => o.pat11).ToString().Substring(0, 6)}");
                Console.WriteLine($"000:\t\t{team.pat000}.\tMax: {allTeams.Max(o => o.pat000)}.\tAvg: {allTeams.Average(o => o.pat000).ToString().Substring(0, 6)}");
                Console.WriteLine($"111:\t\t{team.pat111}.\tMax: {allTeams.Max(o => o.pat111)}.\tAvg: {allTeams.Average(o => o.pat111).ToString().Substring(0, 6)}");
                Console.Read();
            }

            Console.Read();
        }
    }

    public class Pattern
    {

    }

    public class ArchiveMatch
    {
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public DateTime PlayingDate { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
        public string League { get; set; }

        [JsonIgnore]
        public string Result
        {
            get
            {
                if (HomeTeamScore > AwayTeamScore)
                {
                    return "1";
                }
                else if (HomeTeamScore < AwayTeamScore)
                {
                    return "2";
                }
                else
                {
                    return "x";
                }
            }
        }
    }

    public class Team
    {
        public string TeamName { get; set; }
        public int plays { get; set; }
        public int wins { get; set; }
        public int pat00 { get; set; }
        public int pat01 { get; set; }
        public int pat10 { get; set; }
        public int pat11 { get; set; }
        public int pat111 { get; set; }
        public int pat000 { get; set; }
    }
}
