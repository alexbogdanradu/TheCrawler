using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataAnalisys
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Generate Patterns
            List<string> patterns = new List<string>();
            string alphabet = "1x2";
            int numOfLetters = 6;

            for (int i = 2; i <= numOfLetters; i++)
            {
                patterns.AddRange(GeneratePatterns(alphabet, i));
            }

            #endregion

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

        private static IEnumerable<string> GeneratePatterns(string alphabet, int numOfLetters)
        {
            if (numOfLetters <= 0)
            {
                return null;
            }

            List<string> localPatterns = new List<string>();
            StringBuilder workString = new StringBuilder();

            for (int i = 0; i < numOfLetters; i++)
            {
                workString.Append(alphabet[0]);
            }

            localPatterns.Add(workString.ToString());

            bool stay = true;

            while (stay)
            {
                workString.Replace(workString.ToString(), GetNextPattern(workString.Length - 1, workString.ToString(), alphabet));
                localPatterns.Add(workString.ToString());

                int exitCounter = 0;

                for (int j = 0; j < workString.Length; j++)
                {
                    if (workString[j] == alphabet[alphabet.IndexOf(alphabet.Last())])
                    {
                        exitCounter++;
                    }
                }
                if (exitCounter == workString.Length)
                {
                    stay = false;
                }
            }

            return localPatterns;
        }

        private static string GetNextPattern(int index, string actualString, string alphabet)
        {   
            StringBuilder sb = new StringBuilder(actualString);
            char newLetter = GetNextLetter(actualString[index], alphabet);
            if (newLetter == alphabet[0])
            {
                if (index - 1 < 0)
                {
                    sb[index] = newLetter;
                    return sb.ToString();
                }
                else
                {
                    sb[index] = newLetter;
                    return GetNextPattern(index - 1, sb.ToString(), alphabet);
                }
            }
            else
            {
                sb[index] = newLetter;
                return sb.ToString();
            }
        }

        private static char GetNextLetter(char v, string alpha)
        {
            StringBuilder alphabet = new StringBuilder(alpha);

            for (int i = 0; i < alphabet.Length; i++)
            {
                if (alphabet[i] == v)
                {
                    if (i == alphabet.Length - 1)
                    {
                        return alphabet[0];
                    }

                    return alphabet[i + 1];
                }
            }

            return ' ';
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
