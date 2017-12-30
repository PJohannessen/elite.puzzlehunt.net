using System;

namespace PuzzleHunt.Web.Models
{
    public class Guess
    {
        public int Id { get; private set; }
        public DateTime Time { get; private set; }
        public string UserName { get; private set; }
        public string PuzzleName { get; private set; }
        public string Answer { get; private set; }

        public Guess(int id, DateTime time, string userName, string puzzleName, string guess)
        {
            Id = id;
            Time = time;
            UserName = userName;
            PuzzleName = puzzleName;
            Answer = guess;
        }
    }
}