using System;
using System.Web;
using System.Web.Security;

namespace PuzzleHunt.Web.Models
{
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public HttpCookie CreateAuthenticationTicket(int userId, bool createPersistentCookie)
        {
            var ticket = new FormsAuthenticationTicket(1, userId.ToString(), DateTime.Now, DateTime.Now.AddYears(1),
                                                       createPersistentCookie, "");
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            HttpCookie authenticationCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            authenticationCookie.Expires = ticket.Expiration;
            return authenticationCookie;
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}