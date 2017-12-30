using System.Collections.Generic;

namespace PuzzleHunt.Web.Models
{
    public class TeamSummary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserSummary> Players { get; set; }
    }
}