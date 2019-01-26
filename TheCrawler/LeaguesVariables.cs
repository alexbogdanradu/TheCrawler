using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using static TheCrawler.LeagueEnum;

namespace TheCrawler
{
    public static class LeagueEnum
    {
        public enum Leagues
        {
            GBPremierLeague,
            GBChampionship,
            GBLeagueOne,
            GBLeagueTwo,
            GBNationalLeague,
            FRLigue1,
            FRLigue2,
            FRNational,
            DEBundesliga,
            DEBundesliga2,
            DEBundesliga3,
            DERegionalligaNorth,
            DERegionalligaNordost,
            DERegionalligaVest,
            DERegionalligaSudwest,
            DERegionalligaBayern,
            DERegionalligaSouth,
            ITSerieA,
            ITSerieB,
            ROLiga1,
            ROLiga2,
            ESLaLiga,
            IRLPremierDivision,
            NIRLNIFLPremiership,
            BEJupilerLeague,
            ATLaLiga2,
            AUALeague,
            BEProximusLeague,
            ESLaLiga2,
            ARSuperliga,
            PTPrimeiraLiga,
            POEkstraklasa,
            TRSuperLig,
            NLEredivisie,
            RUPremierLeague,
            GRSUperLeague,
        }

        public static Dictionary<Leagues, string> dictLeagues = new Dictionary<Leagues, string>()
        {
            {Leagues.GBPremierLeague, "anglia/premier-league"},
            {Leagues.GBChampionship, "anglia/championship"},
            {Leagues.GBLeagueOne, "anglia/league-one"},
            {Leagues.GBLeagueTwo, "anglia/league-two"},
            {Leagues.GBNationalLeague, "anglia/national-league"},
            {Leagues.FRLigue1, "franta/ligue-1"},
            {Leagues.FRLigue2, "franta/ligue-2"},
            {Leagues.FRNational, "franta/national"},
            {Leagues.DEBundesliga, "germania/bundesliga"},
            {Leagues.DEBundesliga2, "germania/2-bundesliga"},
            {Leagues.DEBundesliga3, "germania/3-liga"},
            {Leagues.DERegionalligaNorth, "germania/regionalliga-north"},
            {Leagues.DERegionalligaNordost, "germania/regionalliga-nordost"},
            {Leagues.DERegionalligaVest, "germania/regionalliga-vest"},
            {Leagues.DERegionalligaSudwest, "germania/regionalliga-sudwest"},
            {Leagues.DERegionalligaBayern, "germania/regionalliga-bayern"},
            {Leagues.DERegionalligaSouth, "germania/regionalliga-south"},
            {Leagues.ITSerieA, "italia/serie-a"},
            {Leagues.ITSerieB, "italia/serie-b"},
            {Leagues.ROLiga1, "romania/liga-1"},
            {Leagues.ROLiga2, "romania/liga-2"},
            {Leagues.ESLaLiga, "spania/laliga"},
            {Leagues.AUALeague, "australia/a-league"},
            {Leagues.ATLaLiga2, "austria/tipico-bundesliga"},
            {Leagues.BEJupilerLeague, "belgia/jupiler-league"},
            {Leagues.BEProximusLeague, "belgia/proximus-league"},
            {Leagues.NIRLNIFLPremiership, "irlanda-de-nord/nifl-premiership"},
            {Leagues.IRLPremierDivision, "irlanda/premier-division"},
            {Leagues.ESLaLiga2, "spania/laliga2"},
            {Leagues.ARSuperliga, "argentina/superliga"},
            {Leagues.PTPrimeiraLiga, "portugalia/primeira-liga"},
            {Leagues.POEkstraklasa, "polonia/ekstraklasa"},
            {Leagues.TRSuperLig, "turcia/super-lig"},
            {Leagues.NLEredivisie, "olanda/eredivisie"},
            {Leagues.RUPremierLeague, "rusia/premier-league"},
            {Leagues.GRSUperLeague, "grecia/super-league"},

        };
    }

    public class Match
    {
        public DateTime dtDateTime { get; set; }
        public string strHomeTeam { get; set; }
        public string strAwayTeam { get; set; }

        public int iHomeScore { get; set; }
        public int iAwayScore { get; set; }

        public string strResultEstimated { get; set; }
        public string strResultActual { get; set; }

    }

    public partial class League
    {
        public LeagueEnum.Leagues currentLeague;
        public DateTime timestamp;
        public List<string> lsStandings;
        public List<Match> lsMatches;
        
        public League()
        {
            this.lsMatches = new List<Match>();
            this.lsStandings = new List<string>();
            this.timestamp = DateTime.Now;
        }

        public void InitLeague(LeagueEnum.Leagues selectedLeague)
        {
            this.currentLeague = selectedLeague;
        }
    }
}
