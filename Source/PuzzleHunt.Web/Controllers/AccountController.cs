using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PuzzleHunt.Web.Models;

namespace PuzzleHunt.Web.Controllers
{
    public class AccountController : PuzzleHuntBaseController
    {
        private IFormsAuthenticationService FormsService { get; set; }
        private IAccountService AccountService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (AccountService == null) { AccountService = new AccountService(); }

            base.Initialize(requestContext);
        }

        [CustomRoute("account")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [CustomRoute("account/login")]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [CustomRoute("account/login")]
        public ActionResult LogIn(LogInModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (AccountService.ValidateUser(model.Username, model.Password))
                {
                    HttpCookie authenticationCookie = FormsService.CreateAuthenticationTicket(AccountService.GetUserId(model.Username), model.RememberMe);
                    Response.Cookies.Add(authenticationCookie);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [CustomRoute("account/logoff")]
        public ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [CustomRoute("account/register")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [CustomRoute("account/register")]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                CreateUserStatus createUserStatus = AccountService.CreateUser(model.Username, model.Password);

                if (createUserStatus == CreateUserStatus.Success)
                {
                    int userId = AccountService.GetUserId(model.Username);
                    HttpCookie authenticationCookie = FormsService.CreateAuthenticationTicket(userId, false);
                    Response.Cookies.Add(authenticationCookie);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createUserStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize]
        [CustomRoute("account/changepassword")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [CustomRoute("account/changepassword")]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (AccountService.ChangePassword(CurrentUser.Id, model.NewPassword))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}
