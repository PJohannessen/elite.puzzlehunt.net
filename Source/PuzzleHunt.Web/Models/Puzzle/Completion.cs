using System;

namespace PuzzleHunt.Web.Models
{
    public class Completion
    {
        public string TeamName { get; private set; }
        public int HintsTaken { get; private set; }
        public TimeSpan TimeSinceLastActivity { get; private set; }

        public Completion(string team, int hintsTaken, TimeSpan timeSinceLastActivity)
        {
            if (team == null) throw new ArgumentNullException("team");
            TeamName = team;
            HintsTaken = hintsTaken;
            TimeSinceLastActivity = timeSinceLastActivity;
        }
    }
}