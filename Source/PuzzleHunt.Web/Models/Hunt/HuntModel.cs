using System;
using System.Collections.Generic;

namespace PuzzleHunt.Web.Models
{
    public class HuntModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public UserSummary Creator { get; set; }
        public IList<PuzzleSummary> Puzzles { get; set; }
        public IList<TeamSummary> Teams { get; set; }
        public UserHuntStatus UserHuntStatus { get; set; }
    }
}