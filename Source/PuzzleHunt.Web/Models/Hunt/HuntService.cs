using System;
using System.Collections.Generic;
using System.Linq;

namespace PuzzleHunt.Web.Models
{
    public class HuntService : IHuntService
    {
        public IEnumerable<HuntSummary> GetHunts()
        {
            foreach (Hunt hunt in Current.DB.Hunts)
            {
                yield return new HuntSummary
                                 {
                                     Id = hunt.Id,
                                     Name = hunt.Name,
                                     Creator = new UserSummary(hunt.User.Id, hunt.User.Username)
                                 };
            }
        }

        public HuntModel GetHuntDetails(int huntId, int? userId)
        {
            Hunt huntData = Current.DB.Hunts.SingleOrDefault(h => h.Id == huntId);
            if (huntData == null) return null;
            DateTime? startTime = null;
            DateTime? completionTime = null;
            IList<HintSummary> hints = null;

            IList<PuzzleSummary> puzzles = huntData.Puzzles.Select(p => new PuzzleSummary
                                                                            {
                                                                                Id = p.Id,
                                                                                Name = p.Name,
                                                                                Difficulty = p.Difficulty,
                                                                                Order = p.Order,
                                                                                Creator = new UserSummary(p.User.Id, p.User.Username),
                                                                                UserPuzzleStatus = GetUserPuzzleStatus(userId, p.Id, out startTime, out completionTime, out hints),
                                                                                StartTime = startTime,
                                                                                CompletionTime = completionTime,
                                                                                Hints = hints
                                                                            }).ToList();
            IList<TeamSummary> teams = huntData.Teams.Select(t => new TeamSummary
                                                                      {
                                                                          Id = t.Id,
                                                                          Name = t.Name,
                                                                          Players = (from membership in Current.DB.TeamMemberships
                                                                                     where membership.TeamId == t.Id
                                                                                     select membership.User)
                                                                                     .Select(user => new UserSummary(user.Id, user.Username)).ToArray()
                                                                      }).ToList();

            UserHuntStatus userHuntStatus = UserHuntStatus.UserNotInTeam;

            if (userId == null)
                userHuntStatus = UserHuntStatus.UserNotFound;
            else if (huntData.CreatorId == userId)
                userHuntStatus = UserHuntStatus.UserIsAdmin;
            else if (huntData.Teams.Any(team => team.TeamMemberships.Any(tm => tm.UserId == userId)))
                userHuntStatus = UserHuntStatus.UserInTeam;

            HuntModel hunt = new HuntModel
                                 {
                                     Id = huntData.Id,
                                     Name = huntData.Name,
                                     StartTime = huntData.StartTime,
                                     EndTime = huntData.EndTime,
                                     Creator = new UserSummary(huntData.User.Id, huntData.User.Username),
                                     Puzzles = puzzles.OrderBy(p => p.Order).ToList(),
                                     Teams = teams,
                                     UserHuntStatus = userHuntStatus
                                 };

            return hunt;
        }

        public CreateHuntStatus CreateHunt(int userId, string huntName)
        {
            if (string.IsNullOrWhiteSpace(huntName)) return CreateHuntStatus.InvalidName;
            if (Current.DB.Hunts.Any(hunt => hunt.Name == huntName)) return CreateHuntStatus.DuplicateName;

            Hunt newHunt = new Hunt {Name = huntName, CreatorId = userId};
            Current.DB.Hunts.InsertOnSubmit(newHunt);
            Current.DB.SubmitChanges();
            return CreateHuntStatus.Success;
        }

        private UserPuzzleStatus GetUserPuzzleStatus(int? userId, int puzzleId, out DateTime? startTime, out DateTime? completionTime, out IList<HintSummary> hints)
        {
            startTime = null;
            completionTime = null;
            hints = new List<HintSummary>();

            if (userId == null)
                return UserPuzzleStatus.UserNotInTeam;

            var team = (from t in Current.DB.Teams
                        where t.TeamMemberships.Any(tm => tm.UserId == userId)
                        select t).SingleOrDefault();
            if (team == null)
                return UserPuzzleStatus.UserNotInTeam;

            var puzzleStatus = team.TeamPuzzleResults.Where(tpr => tpr.PuzzleId == puzzleId).SingleOrDefault();
            if (puzzleStatus == null)
            {
                return UserPuzzleStatus.NotStarted;
            }
            else
            {
                startTime = puzzleStatus.StartTime;
                IList<HintSummary> hintRequests = (from thr in Current.DB.TeamHintRequests
                                                   where thr.TeamId == team.Id
                                                   where thr.Hint.PuzzleId == puzzleId
                                                   select new HintSummary(thr.Hint.Order, thr.RequestTime)).ToArray();
                hints = hintRequests.OrderBy(thr => thr.Order).ToList();
                                   

                if (puzzleStatus.EndTime == null)
                {
                    return UserPuzzleStatus.Started;
                }
                else
                {
                    completionTime = puzzleStatus.EndTime;
                    return UserPuzzleStatus.Completed;
                }
            }
        }
    }
}