namespace PuzzleHunt.Web.Models
{
    public interface IAccountService
    {
        bool ValidateUser(string username, string password);
        int GetUserId(string username);
        CreateUserStatus CreateUser(string username, string password);
        bool ChangePassword(int userId, string newPassword);
    }
}