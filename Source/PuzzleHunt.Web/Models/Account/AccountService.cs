using System;
using System.Collections.Generic;
using System.Linq;

namespace PuzzleHunt.Web.Models
{
    public class AccountService : IAccountService
    {
        private const int PasswordHashWorkFactor = 10;
        private readonly HashSet<char> _WhitedlistedCharacters = new HashSet<char>
                                                                     {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
                                                                      'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                                                                      'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
                                                                      'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                                                                      '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '_', '-', '.', '\'', ' '}; 

        public bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("username");
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("password");

            User userLookup = Current.DB.Users.FirstOrDefault(u => u.Username == username);
            if (userLookup == null) return false;
            return BCrypt.Net.BCrypt.Verify(password, userLookup.Password);
        }

        public int GetUserId(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("username");

            User userLookup = Current.DB.Users.FirstOrDefault(u => u.Username == username);
            if (userLookup == null) throw new Exception("No user with specified username.");
            return userLookup.Id;
        }

        public CreateUserStatus CreateUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                username.Length != username.Trim().Length ||
                username.Any(c => !_WhitedlistedCharacters.Contains(c)))
                return CreateUserStatus.InvalidUsername;

            if (string.IsNullOrEmpty(password)) return CreateUserStatus.InvalidPassword;

            if (Current.DB.Users.Any(u => u.Username == username)) return CreateUserStatus.DuplicateUsername;

            User newUser = new User
                               {
                                   Username = username,
                                   Password = BCrypt.Net.BCrypt.HashPassword(password, PasswordHashWorkFactor),
                               };
            Current.DB.Users.InsertOnSubmit(newUser);
            Current.DB.SubmitChanges();

            return CreateUserStatus.Success;
        }

        public bool ChangePassword(int userId, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword)) throw new ArgumentException("newPassword");
            User userLookup = Current.DB.Users.FirstOrDefault(u => u.Id == userId);
            userLookup.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            Current.DB.SubmitChanges();
            return true;
        }
    }
}