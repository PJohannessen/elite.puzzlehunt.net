using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using PuzzleHunt.Web.Models;

namespace PuzzleHunt.Web.Controllers
{
    public class HuntController : PuzzleHuntBaseController
    {
        /*
        private IHuntService HuntService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (HuntService == null) HuntService = new HuntService();

            base.Initialize(requestContext);
        }

        [CustomRoute("hunts")]
        public ActionResult Index()
        {
            IEnumerable<HuntSummary> hunts = HuntService.GetHunts();
            return View(hunts);
        }

        [HttpGet]
        [CustomRoute("hunts/{id:INT}/{name}")]
        public ActionResult Details(int id, string name)
        {
            int? currentUserId = null;
            if (CurrentUser != null) currentUserId  = CurrentUser.Id;
            HuntModel hunt = HuntService.GetHuntDetails(id, currentUserId);
            if (hunt == null) return RedirectToAction("Index");
            return View(hunt);
        }

        [HttpGet]
        [Authorize]
        [CustomRoute("hunts/create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [CustomRoute("hunts/create")]
        public ActionResult Create(CreateHuntModel createHuntModel)
        {
            if (ModelState.IsValid)
            {
                CreateHuntStatus createHuntStatus = HuntService.CreateHunt(CurrentUser.Id, createHuntModel.Name);
                if (createHuntStatus == CreateHuntStatus.Success)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", createHuntStatus.ToString());
                }
            }

            return View(createHuntModel);
        }*/
    }
}
