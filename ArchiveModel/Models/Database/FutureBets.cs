using System;
using System.Collections.Generic;

namespace ArchiveModel.Models.Database
{
    public partial class FutureBets
    {
        public int Id { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public DateTime PlayingDate { get; set; }
    }
}
