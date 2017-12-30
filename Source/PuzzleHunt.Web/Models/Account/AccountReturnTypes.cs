namespace PuzzleHunt.Web.Models
{
    public enum CreateUserStatus
    {
        Success,
        InvalidUsername,
        InvalidPassword,
        DuplicateUsername,
    }
}