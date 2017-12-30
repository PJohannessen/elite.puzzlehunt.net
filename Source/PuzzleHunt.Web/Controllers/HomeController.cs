using System.Web.Mvc;
using PuzzleHunt.Web.Models;

namespace PuzzleHunt.Web.Controllers
{
    public class HomeController : PuzzleHuntBaseController
    {
        [CustomRoute("")]
        public ActionResult Index()
        {
            return View();
        }

        [CustomRoute("rules")]
        public ActionResult Rules()
        {
            return View();
        }

        [CustomRoute("FAQ")]
        public ActionResult FAQ()
        {
            return View();
        }

        [CustomRoute("about")]
        public ActionResult Contacts()
        {
            return View();
        }
    }
}
