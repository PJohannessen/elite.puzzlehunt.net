using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using PuzzleHunt.Web.Models;
using MvcMiniProfiler;

namespace PuzzleHunt.Web.Controllers
{
    [ValidateInput(false)]
    public abstract class PuzzleHuntBaseController : Controller
    {
        private AuthenticatedUser _CurrentUser;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            ViewBag.CurrentUser = CurrentUser;
        }

        /// <summary>
        /// Gets a User object representing the current request's client.
        /// </summary>
        protected AuthenticatedUser CurrentUser
        {
            get
            {
                if (_CurrentUser == null && Request.IsAuthenticated) InitializeCurrentUser();
                return _CurrentUser;
            }
        }

        private void InitializeCurrentUser()
        {
            if (Request.IsAuthenticated)
            {
                int id;
                if (Int32.TryParse(User.Identity.Name, out id))
                {
                    using (MiniProfiler.Current.Step("Getting current user details."))
                    {
                        User userLookup = Current.DB.Users.FirstOrDefault(u => u.Id == id);
                        if (userLookup != null)
                        {
                            _CurrentUser = new AuthenticatedUser
                            {
                                Id = userLookup.Id,
                                Username = userLookup.Username,
                            };
                        }
                        else
                        {
                            _CurrentUser = new AuthenticatedUser
                            {
                                Id = 0
                            };
                        }
                    }
                }
                else
                {
                    FormsAuthentication.SignOut();
                }
            }
        }

        protected bool IsAuthorized()
        {
            bool authorized = false;
            if (CurrentUser != null && CurrentUser.Id != 0) authorized = true;
            return authorized;
        }

        protected void SetMessage(string message)
        {
            TempData["message"] = message;
        }
    }
}
