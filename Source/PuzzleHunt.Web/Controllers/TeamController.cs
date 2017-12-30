using System.Web.Mvc;
using System.Web.Routing;
using PuzzleHunt.Web.Models;

namespace PuzzleHunt.Web.Controllers
{
    public class TeamController : PuzzleHuntBaseController
    {
        private ITeamService TeamService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (TeamService == null) TeamService = new TeamService();

            base.Initialize(requestContext);
        }
        
        [HttpGet]
        [CustomRoute("teams/create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [CustomRoute("teams/create")]
        public ActionResult Create(CreateTeamModel model)
        {
            if (ModelState.IsValid)
            {
                int userId = CurrentUser.Id;
                CreateTeamResult createTeamResult = TeamService.CreateTeam(1, userId, model.Name, model.Password);
                switch (createTeamResult)
                {
                    case CreateTeamResult.UserDoesNotExist:
                    case CreateTeamResult.HuntDoesNotExist:
                        return RedirectToAction("Index", "Puzzle");
                    case CreateTeamResult.DuplicateName:
                        SetMessage("A team with that name already exists.");
                        return View(model);
                    case CreateTeamResult.PlayerInTeam:
                        SetMessage("You cannot create a team if you are already in one");
                        return View(model);
                    case CreateTeamResult.InvalidTeamName:
                        SetMessage("Your team name must consist of A-Z, a-z, 0-9, underscores, dashes, dots or whitespace and must not start or end with spaces.");
                        return View(model);
                    case CreateTeamResult.Success:
                        SetMessage("You have successfully created a team.");
                        return RedirectToAction("Index", "Puzzle");
                }

            }

            return View(model);
        }

        [HttpGet]
        [CustomRoute("teams/join/{id:INT}")]
        public ActionResult Join(int id)
        {
            int currentUserId;
            if (CurrentUser != null)
                currentUserId = CurrentUser.Id;
            else
                return RedirectToAction("Index", "Puzzle");

            var teamEligibility = TeamService.GetTeamEligibility(id, currentUserId);
            switch (teamEligibility)
            {
                case JoinTeamEligibility.UserDoesNotExist:
                    return RedirectToAction("Index", "Puzzle");
                case JoinTeamEligibility.TeamDoesNotExist:
                    SetMessage("The selected team does not exist.");
                    return RedirectToAction("Index", "Puzzle");
                case JoinTeamEligibility.TeamFull:
                    SetMessage("The selected team is full and cannot be joined.");
                    return RedirectToAction("Index", "Puzzle");
                default:
                    TeamSummary summary = new TeamSummary {Id = id};
                    return View(summary);
            }
        }

        [HttpPost]
        [CustomRoute("teams/join/{id:INT}")]
        public ActionResult Join(int id, string password)
        {
            int currentUserId;
            if (CurrentUser != null)
                currentUserId = CurrentUser.Id;
            else
                return RedirectToAction("Index", "Puzzle");


            TeamSummary model = new TeamSummary { Id = id };
            if (string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("password", "A team must have a password.");
                
                return View(model);
            }

            JoinTeamResult joinTeamResult = TeamService.JoinTeam(id, currentUserId, password);

            ActionResult actionResult;
            switch (joinTeamResult)
            {
                case JoinTeamResult.TeamDoesNotExist:
                    SetMessage("The selected team does not exist.");
                    actionResult = RedirectToAction("Index", "Puzzle");
                    break;
                case JoinTeamResult.UserInTeam:
                    SetMessage("You cannot join more than one team.");
                    actionResult = RedirectToAction("Index", "Puzzle");
                    break;
                case JoinTeamResult.TeamFull:
                    SetMessage("The selected team is full and cannot be joined.");
                    actionResult = RedirectToAction("Index", "Puzzle");
                    break;
                case JoinTeamResult.PasswordIncorrect:
                    SetMessage("The specified password was incorrect.");
                    actionResult = View(model);
                    break;
                case JoinTeamResult.Success:
                    SetMessage("You have now joined a team and may compete in the hunt!");
                    actionResult = RedirectToAction("Index", "Puzzle");
                    break;
                case JoinTeamResult.UserDoesNotExist:
                default:
                    actionResult = RedirectToAction("Index", "Puzzle");
                    break;
            }

            return actionResult;
        }
    }
}
