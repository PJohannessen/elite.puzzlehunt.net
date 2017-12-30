using System.Web;

namespace PuzzleHunt.Web.Models
{
    public interface IFormsAuthenticationService
    {
        HttpCookie CreateAuthenticationTicket(int userId, bool createPersistentCookie);
        void SignOut();
    }
}