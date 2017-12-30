namespace PuzzleHunt.Web.Models
{
    public static class AccountValidation
    {
        public static string ErrorCodeToString(CreateUserStatus createUserStatus)
        {
            switch (createUserStatus)
            {
                case CreateUserStatus.DuplicateUsername:
                    return "Username already exists. Please enter a different user name.";

                case CreateUserStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case CreateUserStatus.InvalidUsername:
                    return "The user name provided is invalid. Please check the value and try again.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }
}