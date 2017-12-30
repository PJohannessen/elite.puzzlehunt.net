using System;
using System.Collections.Generic;

namespace PuzzleHunt.Web.Models
{
    public class PuzzleSummary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserSummary Creator { get; set; }
        public int Order { get; set; }
        public string Difficulty { get; set; }
        public UserPuzzleStatus UserPuzzleStatus { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? CompletionTime { get; set; }
        public IList<HintSummary> Hints { get; set; }
    }
}