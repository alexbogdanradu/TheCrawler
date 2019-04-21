using System;
using System.Collections.Generic;
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
            //FRLnb,
            //ITLigaA,
            //RODiviziaA,
            //ESAcb,
            //USANba,
            //TRSuperLigi,
            //DEBbl,
            //USANcaa,
        }

        public static Dictionary<Leagues, string> dictLeagues = new Dictionary<Leagues, string>()
        {
            {Leagues.GBPremierLeague, "fotbal/anglia/premier-league"},
            {Leagues.GBChampionship, "fotbal/anglia/championship"},
            {Leagues.GBLeagueOne, "fotbal/anglia/league-one"},
            {Leagues.GBLeagueTwo, "fotbal/anglia/league-two"},
            {Leagues.GBNationalLeague, "fotbal/anglia/national-league"},
            {Leagues.FRLigue1, "fotbal/franta/ligue-1"},
            {Leagues.FRLigue2, "fotbal/franta/ligue-2"},
            {Leagues.FRNational, "fotbal/franta/national"},
            {Leagues.DEBundesliga, "fotbal/germania/bundesliga"},
            {Leagues.DEBundesliga2, "fotbal/germania/2-bundesliga"},
            {Leagues.DEBundesliga3, "fotbal/germania/3-liga"},
            {Leagues.DERegionalligaNorth, "fotbal/germania/regionalliga-north"},
            {Leagues.DERegionalligaNordost, "fotbal/germania/regionalliga-nordost"},
            {Leagues.DERegionalligaVest, "fotbal/germania/regionalliga-vest"},
            {Leagues.DERegionalligaSudwest, "fotbal/germania/regionalliga-sudwest"},
            {Leagues.DERegionalligaBayern, "fotbal/germania/regionalliga-bayern"},
            {Leagues.DERegionalligaSouth, "fotbal/germania/regionalliga-south"},
            {Leagues.ITSerieA, "fotbal/italia/serie-a"},
            {Leagues.ITSerieB, "fotbal/italia/serie-b"},
            {Leagues.ROLiga1, "fotbal/romania/liga-1"},
            {Leagues.ROLiga2, "fotbal/romania/liga-2"},
            {Leagues.ESLaLiga, "fotbal/spania/laliga"},
            {Leagues.AUALeague, "fotbal/australia/a-league"},
            {Leagues.ATLaLiga2, "fotbal/austria/tipico-bundesliga"},
            {Leagues.BEJupilerLeague, "fotbal/belgia/jupiler-league"},
            {Leagues.BEProximusLeague, "fotbal/belgia/proximus-league"},
            {Leagues.NIRLNIFLPremiership, "fotbal/irlanda-de-nord/nifl-premiership"},
            {Leagues.IRLPremierDivision, "fotbal/irlanda/premier-division"},
            {Leagues.ESLaLiga2, "fotbal/spania/laliga2"},
            {Leagues.ARSuperliga, "fotbal/argentina/superliga"},
            {Leagues.PTPrimeiraLiga, "fotbal/portugalia/primeira-liga"},
            {Leagues.POEkstraklasa, "fotbal/polonia/ekstraklasa"},
            {Leagues.TRSuperLig, "fotbal/turcia/super-lig"},
            {Leagues.NLEredivisie, "fotbal/olanda/eredivisie"},
            {Leagues.RUPremierLeague, "fotbal/rusia/premier-league"},
            {Leagues.GRSUperLeague, "fotbal/grecia/super-league"},
            //{Leagues.FRLnb, "baschet/franta/lnb"},
            //{Leagues.ITLigaA, "baschet/italia/liga-a"},
            //{Leagues.RODiviziaA, "baschet/romania/divizia-a"},
            //{Leagues.ESAcb, "baschet/spania/acb"},
            //{Leagues.USANba, "baschet/sua/nba"},
            //{Leagues.TRSuperLigi, "baschet/turcia/super-ligi"},
            //{Leagues.DEBbl, "baschet/germania/bbl"},
            //{Leagues.USANcaa, "baschet/sua/ncaa"},

            //{Leagues.NONE, "placeholder"},
        };
    }

    public class Match
    {
        public DateTime dtDateTime { get; set; }
        public LeagueEnum.Leagues matchLeague;
        public string strGameType { get; set; }
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
