using System;
using System.Collections.Generic;

namespace ArchiveModel.Models.Database
{
    public partial class Archive
    {
        public int IdArchive { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public DateTime PlayingDate { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
        public string League { get; set; }
    }
}
