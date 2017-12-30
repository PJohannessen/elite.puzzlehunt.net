using System.Collections.Generic;

namespace PuzzleHunt.Web.Models
{
    public class AdminDetails
    {
        public bool IsAdmin { get; private set; }
        public IList<Guess> Guesses { get; private set; }

        public AdminDetails(bool isAdmin, IList<Guess> guesses)
        {
            IsAdmin = isAdmin;
            Guesses = guesses;
        }
    }
}