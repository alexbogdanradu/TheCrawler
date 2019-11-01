using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace DataAnalisys
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            #region Generate Patterns
            List<string> patterns = new List<string>();
            string alphabet = "1x2";
            int startQuantityPattern = 3;
            int endQuantityPattern = 3;

            for (int i = startQuantityPattern; i <= endQuantityPattern; i++)
            {
                patterns.AddRange(GeneratePatterns(alphabet, i));
            }

            #endregion

            #region GetAllTeams
            List<ArchiveMatch> matches = new List<ArchiveMatch>();

            //get the matches from the file
            using (StreamReader sr = new StreamReader("allLeagues.json"))
            {
                matches.AddRange(JsonConvert.DeserializeObject<List<ArchiveMatch>>(sr.ReadToEnd()));
            }

            //Get all unique teams
            List<string> teams = matches.Select(o => o.HomeTeam).Distinct().ToList();
            teams.AddRange(matches.Select(o => o.AwayTeam).Distinct().ToList());
            teams = teams.Distinct().ToList();

            //dictionary containing name of the team and its played matches
            Dictionary<string, List<ArchiveMatch>> matchesPlayedByAteam = new Dictionary<string, List<ArchiveMatch>>();

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

            #endregion

            #region Analyze Pattern Occurances

            List<Pattern> occurances = new List<Pattern>();
            List<Team> teamsWithPatterns = new List<Team>();

            //foreach team
            foreach (var teamMatchesDict in matchesPlayedByAteam)
            {
                Team team = new Team { Name = teamMatchesDict.Key};

                Console.Clear();
                foreach (var item in occurances.OrderBy(o => o.occurance).Reverse().Take(30))
                {
                    Console.WriteLine($"{item.pattern}: {item.occurance}");
                }

                foreach (var pattern in patterns)
                {
                    string secondPattern = "";
                    int count = 0;

                    //search for pattern
                    for (int i = 0; i < teamMatchesDict.Value.Count - pattern.Length; i++)
                    {
                        for (int j = 0; j < pattern.Length; j++)
                        {
                            if (teamMatchesDict.Value[i + j].HomeTeam == teamMatchesDict.Key)
                            {
                                secondPattern += teamMatchesDict.Value[i + j].Result;
                            }
                            else
                            {
                                secondPattern += teamMatchesDict.Value[i + j].ResultAsAwayTeam;
                            }
                        }

                        if (secondPattern == pattern)
                        {
                            count++;
                        }
                    }

                    //save the occurances
                    if (!team.patterns.Any(o => o.pattern == pattern))
                    {
                        team.patterns.Add(new Pattern { pattern = pattern, occurance = count });
                    }
                    else
                    {
                        team.patterns.First(o => o.pattern == pattern).occurance += count;
                    }
                }

                //save to teams list
                teamsWithPatterns.Add(team);
            }

            #endregion

            //foreach (var team in allTeams)
            //{
            //    Console.Clear();
            //    Console.WriteLine($"Team:\t\t{team.TeamName}");
            //    Console.WriteLine($"Games played:\t{team.plays}");
            //    Console.WriteLine($"00:\t\t{team.pat00}.\tMax: {allTeams.Max(o => o.pat00)}.\tAvg: {allTeams.Average(o => o.pat00).ToString().Substring(0, 6)}");
            //    Console.WriteLine($"01:\t\t{team.pat01}.\tMax: {allTeams.Max(o => o.pat01)}.\tAvg: {allTeams.Average(o => o.pat01).ToString().Substring(0, 6)}");
            //    Console.WriteLine($"10:\t\t{team.pat10}.\tMax: {allTeams.Max(o => o.pat10)}.\tAvg: {allTeams.Average(o => o.pat10).ToString().Substring(0, 6)}");
            //    Console.WriteLine($"11:\t\t{team.pat11}.\tMax: {allTeams.Max(o => o.pat11)}.\tAvg: {allTeams.Average(o => o.pat11).ToString().Substring(0, 6)}");
            //    Console.WriteLine($"000:\t\t{team.pat000}.\tMax: {allTeams.Max(o => o.pat000)}.\tAvg: {allTeams.Average(o => o.pat000).ToString().Substring(0, 6)}");
            //    Console.WriteLine($"111:\t\t{team.pat111}.\tMax: {allTeams.Max(o => o.pat111)}.\tAvg: {allTeams.Average(o => o.pat111).ToString().Substring(0, 6)}");
            //    Console.Read();
            //}

            Console.WriteLine(sw.Elapsed);
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
        public string pattern;
        public int occurance;
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
        public string ResultAsAwayTeam
        {
            get
            {
                if (HomeTeamScore > AwayTeamScore)
                {
                    return "2";
                }
                else if (HomeTeamScore < AwayTeamScore)
                {
                    return "1";
                }
                else
                {
                    return "x";
                }
            }
        }

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
        public string Name;
        public int Wins;
        public int Losses;
        public int Draws;

        public List<Pattern> patterns = new List<Pattern>();

        public int playedMatches { get {
                return Wins + Losses + Draws;
            } }
    }
}
