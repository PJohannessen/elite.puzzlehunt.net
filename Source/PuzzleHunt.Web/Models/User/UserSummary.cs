using System;

namespace PuzzleHunt.Web.Models
{
    public class UserSummary
    {
        public int Id { get; private set; }
        public string Username { get; private set; }

        public UserSummary(int id, string username)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException("username");

            Id = id;
            Username = username;
        }
    }
}