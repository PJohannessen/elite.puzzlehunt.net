 using System.Web.Mvc;
using System.Web.Routing;
using PuzzleHunt.Web.Models;

namespace PuzzleHunt.Web.Controllers
{
    public class PuzzleController : PuzzleHuntBaseController
    {
        private IHuntService HuntService { get; set; }
        private IPuzzleService PuzzleService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (HuntService == null) HuntService = new HuntService();
            if (PuzzleService == null) PuzzleService = new PuzzleService();

            base.Initialize(requestContext);
        }

        [HttpGet]
        [CustomRoute("puzzles")]
        public ActionResult Index()
        {
            int? currentUserId = null;
            if (CurrentUser != null) currentUserId = CurrentUser.Id;
            // Note - Hardcoded to Elite Puzzle Hunt 2011
            HuntModel hunt = HuntService.GetHuntDetails(1, currentUserId);
            if (hunt == null) return RedirectToAction("Index", "Home");
            return View(hunt);
        }

        [HttpGet]        
        [CustomRoute("puzzles/{id:INT}")]
        public ActionResult Details(int id)
        {
            if (!IsAuthorized()) return RedirectToAction("LogIn", "Account");

            int currentUserId;

            if (CurrentUser != null)
                currentUserId = CurrentUser.Id;
            else
                return RedirectToAction("Index");

            PuzzleDetailsModel puzzleDetails = PuzzleService.GetPuzzleDetailsAndStatus(id, currentUserId);

            ActionResult result;
            switch (puzzleDetails.Status)
            {
                case PuzzleStatus.PuzzleDoesNotExist:
                    SetMessage("The requested puzzle does not exist.");
                    result = RedirectToAction("Index");
                    break;
                case PuzzleStatus.HuntNotStarted:
                    SetMessage("The requested puzzle cannot be viewed until the hunt has begun.");
                    result = RedirectToAction("Index");
                    break;
                case PuzzleStatus.UserNotInTeam:
                    SetMessage("You must be in a team to view active puzzles.");
                    result = RedirectToAction("Index");
                    break;
                default:
                    result = View(puzzleDetails);
                    break;
            }
            return result;
        }

        [HttpPost]
        [CustomRoute("puzzles/{id:INT}")]
        public ActionResult Details(int id, string answer)
        {
            if (!IsAuthorized()) return RedirectToAction("LogIn", "Account");

            int currentUserId;
            if (CurrentUser != null)
                currentUserId = CurrentUser.Id;
            else
                return RedirectToAction("Index");

            AnswerPuzzleResult answerResult = PuzzleService.Answer(id, currentUserId, answer);
            
            switch (answerResult)
            {
                case AnswerPuzzleResult.Success:
                    SetMessage("Answer correct.");
                    return RedirectToAction("Index");
                case AnswerPuzzleResult.Incorrect:
                    SetMessage("Answer incorrect.");
                    return RedirectToAction("Details", new { id = id });
                case AnswerPuzzleResult.UserNotInTeam:
                    SetMessage("User not in team.");
                    return RedirectToAction("Details", new { id = id });
                case AnswerPuzzleResult.PuzzleDoesNotExist:
                    SetMessage("Puzzle does not exist.");
                    return RedirectToAction("Details", new { id = id });
            }

            return RedirectToAction("Details", new {id = id});
        }

        [HttpPost]
        [CustomRoute("hint/{id:INT}")]
        public ActionResult Hint(int id)
        {
            int currentUserId;
            if (CurrentUser != null)
                currentUserId = CurrentUser.Id;
            else
                return RedirectToAction("Index");

            UnlockHintResult result = PuzzleService.UnlockHint(id, currentUserId);

            switch (result)
            {
                case UnlockHintResult.NoHintsRemaining:
                    SetMessage("No hints left to unlock");
                    return RedirectToAction("Details", "Puzzle", new { id = id });
                case UnlockHintResult.Success:
                    return RedirectToAction("Details", "Puzzle", new {id = id});
                case UnlockHintResult.PuzzleDoesNotExist:
                default:
                    return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [CustomRoute("puzzles/create/{id:INT}")]
        public ActionResult Create(int id)
        {
            if (!IsAuthorized()) RedirectToAction("LogIn", "Account");

            int? currentUserId = null;
            if (CurrentUser != null) currentUserId = CurrentUser.Id;
            HuntModel huntModel = HuntService.GetHuntDetails(id, currentUserId);
            if (huntModel == null) return RedirectToAction("Index");
            if (huntModel.Creator.Id != currentUserId)
            {
                SetMessage("You do not have permission to create puzzles for the specified hunt.");
                return RedirectToAction("Index");
            }
            var createPuzzleModel = new CreatePuzzleModel {HuntId = id};
            return View(createPuzzleModel);
        }

        [HttpPost]
        [CustomRoute("puzzles/create/{id:INT}")]
        public ActionResult Create(int id, CreatePuzzleModel model)
        {
            if (!IsAuthorized()) return RedirectToAction("LogIn", "Account");

            if (ModelState.IsValid)
            {
                CreatePuzzleResult createPuzzleResult = PuzzleService.Create(id, CurrentUser.Id, model);
                if (createPuzzleResult == CreatePuzzleResult.Success)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", createPuzzleResult.ToString());
                }
            }

            model.HuntId = id;
            return View(model);
        }

        [HttpGet]
        [CustomRoute("puzzles/edit/{id:INT}")]
        public ActionResult Edit(int id)
        {
            if (!IsAuthorized()) return RedirectToAction("LogIn", "Account");

            int userId = CurrentUser.Id;
            var details = PuzzleService.GetPuzzleForEdit(id, userId);
           
            if (details == null)
            {
                SetMessage("The selected puzzle does not exist or you do not have permission to edit it.");
                return RedirectToAction("Index");
            }

            var model = new EditPuzzleModel
                            {
                                Id = details.Id,
                                Name = details.Name,
                                Content = details.Content,
                                Answer = details.Answer,
                                Order = details.Order,
                                Difficulty = details.Difficulty,
                                Solution = details.Solution
                            };
            return View(model);
        }

        [HttpPost]
        [CustomRoute("puzzles/edit/{id:INT}")]
        public ActionResult Edit(int id, EditPuzzleModel model)
        {
            if (!IsAuthorized()) return RedirectToAction("LogIn", "Account");

            if (ModelState.IsValid)
            {
                EditPuzzleResult editPuzzleResult = PuzzleService.Edit(CurrentUser.Id, model);
                if (editPuzzleResult == EditPuzzleResult.Success)
                {
                    return RedirectToAction("Index");
                    // NOTE - Do properly
                    //return RedirectToAction("Details", "Hunt", new { id = id});
                }
                else
                {
                    ModelState.AddModelError("", editPuzzleResult.ToString());
                }
            }

            return View(model);
        }

        [HttpGet]
        [CustomRoute("solutions/{id:INT}")]
        public ActionResult Solution(int id)
        {
            if (!IsAuthorized()) return RedirectToAction("LogIn", "Account");

            PuzzleSolutionModel solutionModel = PuzzleService.GetPuzzleSolution(id);
            if (solutionModel.HuntIsOver && solutionModel.Puzzle == null)
            {
                SetMessage("There is no puzzle with that id.");
                return RedirectToAction("Index");
            }
            if (!solutionModel.HuntIsOver && solutionModel == null)
            {
                SetMessage("Solutions cannot be viewed until the hunt is over");
                return RedirectToAction("Index");
            }
            else
            {
                return View(solutionModel.Puzzle);
            }
        }

        [HttpGet]
        [CustomRoute("admin")]
        public ActionResult Admin()
        {
            if (!IsAuthorized()) return RedirectToAction("LogIn", "Account");
            int userId = CurrentUser.Id;
            AdminDetails adminDetails = PuzzleService.GetAdminDetails(userId);
            if (!adminDetails.IsAdmin)
            {
                return RedirectToAction("Index");
            }
            return View(adminDetails.Guesses);
        }
    }
}
