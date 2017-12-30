using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PuzzleHunt.Web.Models
{
    public class PuzzleService : IPuzzleService
    {
        public CreatePuzzleResult Create(int huntId, int userId, CreatePuzzleModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            var selectedHunt = (from h in Current.DB.Hunts
                                where h.Id == huntId
                                select h).SingleOrDefault();
            if (selectedHunt == null) return CreatePuzzleResult.HuntDoesNotExist;
            if (selectedHunt.CreatorId != userId) return CreatePuzzleResult.UserNotAdmin;

            Puzzle newPuzzle = new Puzzle
                                   {
                                       HuntId = huntId,
                                       CreatorId = userId,
                                       Name = model.Name,
                                       Content = model.Content,
                                       Answer = model.Answer,
                                       Difficulty = model.Difficulty,
                                       Solution = model.Solution,
                                       Order = model.Order
                                   };
            Current.DB.Puzzles.InsertOnSubmit(newPuzzle);
            Current.DB.SubmitChanges();
            return CreatePuzzleResult.Success;
        }

        public EditPuzzleResult Edit(int userId, EditPuzzleModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            var existingPuzzle = (from p in Current.DB.Puzzles
                                  where p.Id == model.Id
                                  select p).SingleOrDefault();
            
            if (existingPuzzle == null) return EditPuzzleResult.PuzzleDoesNotExist;
            if (existingPuzzle.Hunt.CreatorId != userId) return EditPuzzleResult.UserNotAdmin;

            existingPuzzle.Name = model.Name;
            existingPuzzle.Content = model.Content;
            existingPuzzle.Answer = model.Answer;
            existingPuzzle.Difficulty = model.Difficulty;
            existingPuzzle.Solution = model.Solution;
            existingPuzzle.Order = model.Order;

            Current.DB.SubmitChanges();
            return EditPuzzleResult.Success;
        }

        public AnswerPuzzleResult Answer(int puzzleId, int userId, string answer)
        {
            var selectedPuzzle = (from puzzle in Current.DB.Puzzles
                                  where puzzle.Id == puzzleId
                                  select puzzle).SingleOrDefault();

            if (selectedPuzzle == null) return AnswerPuzzleResult.PuzzleDoesNotExist;

            var usersTeam = (from team in Current.DB.Teams
                             where team.HuntId == selectedPuzzle.HuntId
                             where team.TeamMemberships.Any(membership => membership.UserId == userId)
                             select team).SingleOrDefault();

            if (usersTeam == null) return AnswerPuzzleResult.UserNotInTeam;

            var puzzleResult = (from tpr in Current.DB.TeamPuzzleResults
                                where tpr.PuzzleId == puzzleId
                                where tpr.TeamId == usersTeam.Id
                                select tpr).SingleOrDefault();

            if (puzzleResult == null)
                return AnswerPuzzleResult.NoPuzzleResultRecord;
            if (puzzleResult.EndTime != null)
                return AnswerPuzzleResult.PuzzleAlreadyCompleted;
            if (string.IsNullOrWhiteSpace(answer))
            {
                return AnswerPuzzleResult.Incorrect;
            }


            bool answeredCorrectly = string.Equals(Regex.Replace(answer, @"\s+", ""), Regex.Replace(selectedPuzzle.Answer, @"\s+", ""), StringComparison.InvariantCultureIgnoreCase);

            if (answer.Length < 64)
            {
                TeamGuess guess = new TeamGuess
                                      {UserId = userId, PuzzleId = puzzleId, Time = DateTime.UtcNow, Guess = answer};
                Current.DB.TeamGuesses.InsertOnSubmit(guess);
                Current.DB.SubmitChanges();
            }

            if (!answeredCorrectly)
            {
                return AnswerPuzzleResult.Incorrect;
            }
            else
            {
                puzzleResult.EndTime = DateTime.UtcNow;
                Current.DB.SubmitChanges();
                return AnswerPuzzleResult.Success;
            }
        }

        public PuzzleDetailsModel GetPuzzleDetailsAndStatus(int puzzleId, int userId)
        {
            PuzzleModel puzzleDetails = null;
            PuzzleStatus status = PuzzleStatus.PuzzleDoesNotExist;
            IList<HintModel> hints = new List<HintModel>();
            IList<Completion> completions = new List<Completion>();
            Puzzle puzzle = Current.DB.Puzzles.SingleOrDefault(p => p.Id == puzzleId);
            
            if (puzzle == null)
            {
                status = PuzzleStatus.PuzzleDoesNotExist;
            }
            else
            {
                int creatorId = puzzle.Hunt.CreatorId;

                var team = (from t in Current.DB.Teams
                            where t.HuntId == puzzle.HuntId
                            where t.TeamMemberships.Any(membership => membership.UserId == userId)
                            select t).SingleOrDefault();

                if (team == null && creatorId != userId)
                {
                    status = PuzzleStatus.UserNotInTeam;
                }
                else if (puzzle.Hunt.StartTime > DateTime.UtcNow && creatorId != userId)
                {
                    status = PuzzleStatus.HuntNotStarted;
                }
                else
                {
                    puzzleDetails = new PuzzleModel
                    {
                        Id = puzzle.Id,
                        Name = puzzle.Name,
                        Content = puzzle.Content,
                        Creator = new UserSummary(puzzle.User.Id, puzzle.User.Username),
                        Difficulty = puzzle.Difficulty
                    };

                    if (puzzle.Hunt.EndTime < DateTime.UtcNow || creatorId == userId)
                    {
                        status = PuzzleStatus.HuntFinished;

                        if (creatorId == userId)
                        {
                            status = PuzzleStatus.UserIsAdmin;
                        }

                        hints = (from h in Current.DB.Hints
                                 where h.PuzzleId == puzzleId
                                 select new HintModel(h.Id, h.Order, h.Content)).ToList();
                        hints = hints.OrderBy(h => h.Order).ToList();

                        var puzzleResults = from tpr in Current.DB.TeamPuzzleResults
                                            where tpr.EndTime != null
                                            where tpr.PuzzleId == puzzleId
                                            select tpr;
                        if (puzzleResults.Count() > 0)
                        {
                            foreach (var puzzleResult in puzzleResults)
                            {
                                int teamId = puzzleResult.Team.Id;
                                string teamName = puzzleResult.Team.Name;
                                var hintsTaken = from thr in Current.DB.TeamHintRequests
                                                 where thr.TeamId == teamId
                                                 where thr.Hint.PuzzleId == puzzleId
                                                 select thr;
                                int numHintsTaken = hintsTaken.Count();
                                TimeSpan timeSinceActivity;
                                if (numHintsTaken < 1)
                                {
                                    timeSinceActivity = puzzleResult.EndTime.Value - puzzleResult.StartTime;
                                }
                                else
                                {
                                    var latestHintRequest = hintsTaken.OrderBy(thr => thr.Hint.Order).ToArray().Last();
                                    timeSinceActivity = puzzleResult.EndTime.Value - latestHintRequest.RequestTime;
                                }


                                Completion completion = new Completion(teamName, numHintsTaken, timeSinceActivity);
                                completions.Add(completion);
                            }
                        }
                    }
                    else
                    {
                        var teamPuzzleResult = (from result in team.TeamPuzzleResults
                                                where result.PuzzleId == puzzleId
                                                where result.TeamId == team.Id
                                                select result).SingleOrDefault();
                        if (teamPuzzleResult == null)
                        {
                            teamPuzzleResult = new TeamPuzzleResult
                            {
                                PuzzleId = puzzleId,
                                TeamId = team.Id,
                                StartTime = DateTime.UtcNow
                            };
                            Current.DB.TeamPuzzleResults.InsertOnSubmit(teamPuzzleResult);
                            Current.DB.SubmitChanges();
                        }

                        if (teamPuzzleResult.EndTime == null)
                        {
                            status = PuzzleStatus.Started;
                            var hintData = (from h in Current.DB.TeamHintRequests
                                            where h.TeamId == team.Id
                                            where h.Hint.PuzzleId == puzzleId
                                            select h.Hint).ToArray();

                            hints = hintData.Select(h => new HintModel(h.Id, h.Order, h.Content)).OrderBy(h => h.Order).ToList();
                        }
                        else
                        {
                            status = PuzzleStatus.Completed;
                            var hintData = (from h in Current.DB.Hints
                                            where h.PuzzleId == puzzleId
                                            select h).ToArray();
                            hints = hintData.Select(h => new HintModel(h.Id, h.Order, h.Content)).OrderBy(h => h.Order).ToList();

                            var puzzleResults = from tpr in Current.DB.TeamPuzzleResults
                                                where tpr.EndTime != null
                                                where tpr.PuzzleId == puzzleId
                                                select tpr;
                            if (puzzleResults.Count() > 0)
                            {
                                foreach (var puzzleResult in puzzleResults)
                                {
                                    int teamId = puzzleResult.Team.Id;
                                    string teamName = puzzleResult.Team.Name;
                                    var hintsTaken = from thr in Current.DB.TeamHintRequests
                                                     where thr.TeamId == teamId
                                                     where thr.Hint.PuzzleId == puzzleId
                                                     select thr;
                                    int numHintsTaken = hintsTaken.Count();
                                    TimeSpan timeSinceActivity;
                                    if (numHintsTaken < 1)
                                    {
                                        timeSinceActivity = puzzleResult.EndTime.Value - puzzleResult.StartTime;
                                    }
                                    else
                                    {
                                        var latestHintRequest = hintsTaken.OrderBy(thr => thr.Hint.Order).ToArray().Last();
                                        timeSinceActivity = puzzleResult.EndTime.Value - latestHintRequest.RequestTime;
                                    }


                                    Completion completion = new Completion(teamName, numHintsTaken, timeSinceActivity);
                                    completions.Add(completion);
                                }
                            }
                        }
                    }
                }
            }

            completions = completions.OrderBy(comp => comp.HintsTaken).ThenBy(comp => comp.TimeSinceLastActivity).ToList();
            PuzzleDetailsModel puzzleDetailsAndStatus = new PuzzleDetailsModel(status, puzzleDetails, hints, completions);
            return puzzleDetailsAndStatus;
        }

        public EditPuzzleDetails GetPuzzleForEdit(int puzzleId, int userId)
        {
            EditPuzzleDetails details = null;
            Puzzle puzzle = Current.DB.Puzzles.SingleOrDefault(p => p.Id == puzzleId);
            
            if (puzzle != null && puzzle.Hunt.CreatorId == userId)
            {
                details = new EditPuzzleDetails(puzzle.Id, puzzle.Name, puzzle.Content, puzzle.Answer, puzzle.Order, puzzle.Difficulty, puzzle.Solution);
            }

            return details;
        }

        public UnlockHintResult UnlockHint(int puzzleId, int userId)
        {
            var selectedPuzzle = (from p in Current.DB.Puzzles
                                  where p.Id == puzzleId
                                  select p).SingleOrDefault();
            
            if (selectedPuzzle == null) return UnlockHintResult.PuzzleDoesNotExist;

            int teamId = (from t in Current.DB.Teams
                          where t.HuntId == selectedPuzzle.HuntId
                          where t.TeamMemberships.Any(tm => tm.UserId == userId)
                          select t.Id).Single();

            var requestedHintIds = (from thr in Current.DB.TeamHintRequests
                                  where thr.Hint.PuzzleId == puzzleId
                                  where thr.TeamId == teamId                           
                                  select thr.HintId).ToArray();

            int nextAvailableHintId = (from h in Current.DB.Hints
                                       where h.PuzzleId == puzzleId
                                       where !requestedHintIds.Contains(h.Id)
                                       orderby h.Order
                                       select h.Id).FirstOrDefault();

            if (nextAvailableHintId == 0) return UnlockHintResult.NoHintsRemaining;

            TeamHintRequest request = new TeamHintRequest
                                          {
                                              HintId = nextAvailableHintId,
                                              TeamId = teamId,
                                              RequestTime = DateTime.UtcNow
                                          };

            Current.DB.TeamHintRequests.InsertOnSubmit(request);
            Current.DB.SubmitChanges();
            return UnlockHintResult.Success;
        }

        public AdminDetails GetAdminDetails(int userId)
        {
            Hunt hunt = (from h in Current.DB.Hunts
                         select h).FirstOrDefault();
            if (hunt == null || hunt.CreatorId != userId)
            {
                return new AdminDetails(false, new List<Guess>());
            }
            else
            {
                var guesses = (from tg in Current.DB.TeamGuesses
                               select new Guess(tg.Id, tg.Time, tg.User.Username, tg.Puzzle.Name, tg.Guess)).ToList();
                return new AdminDetails(true, guesses);
            }
        }

        public PuzzleSolutionModel GetPuzzleSolution(int puzzleId)
        {
            var puzzle = (from p in Current.DB.Puzzles
                          where p.Id == puzzleId
                          select p).SingleOrDefault();
            if (puzzle == null) return new PuzzleSolutionModel(true, null);
            if (puzzle.Hunt.EndTime > DateTime.UtcNow) return new PuzzleSolutionModel(false, null);
            return new PuzzleSolutionModel(true, new PuzzleWithSolution(puzzle.Name, puzzle.Solution));
        }
    }
} 