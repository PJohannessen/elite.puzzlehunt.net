using System.Collections.Generic;
using System.Linq;

namespace PuzzleHunt.Web.Models
{
    public class TeamService : ITeamService
    {
        private const int PasswordHashWorkFactor = 10;
        private readonly HashSet<char> _WhitedlistedCharacters = new HashSet<char>
                                                                     {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
                                                                      'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                                                                      'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
                                                                      'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                                                                      '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '_', '-', '.', '\'', ' '}; 

        public TeamSummary GetTeam(int id)
        {
            var team = (from t in Current.DB.Teams
                        where t.Id == id
                        select t).SingleOrDefault();

            if (team == null) return null;

            return new TeamSummary
                       {
                           Id = team.Id,
                           Name = team.Name,
                           Players = (from membership in Current.DB.TeamMemberships
                                      where membership.TeamId == team.Id
                                      select membership.User).Select(user => new UserSummary(user.Id, user.Username)).ToArray()
                       };
        }

        public JoinTeamEligibility GetTeamEligibility(int id, int userId)
        {
            var team = (from t in Current.DB.Teams
                        where t.Id == id
                        select t).SingleOrDefault();
            if (team == null) return JoinTeamEligibility.TeamDoesNotExist;
            var user = (from u in Current.DB.Users
                        where u.Id == userId
                        select u).SingleOrDefault();
            if (user == null) return JoinTeamEligibility.UserDoesNotExist;
            if (team.TeamMemberships.Count >= 2) return JoinTeamEligibility.TeamFull;
            if (user.TeamMemberships.Where(tm => tm.Team.HuntId == team.HuntId).Any())
                return JoinTeamEligibility.UserInTeam;
            return JoinTeamEligibility.OK;
        }

        public IEnumerable<TeamSummary> GetTeams(int huntId)
        {
            var teamsInHunt = from team in Current.DB.Teams
                              where team.HuntId == huntId
                              select team;

            foreach (Team team in teamsInHunt)
            {
                int teamId = team.Id;
                yield return new TeamSummary
                {
                    Id = teamId,
                    Name = team.Name,
                    Players = (from membership in Current.DB.TeamMemberships
                               where membership.TeamId == teamId
                               select membership.User).Select(user => new UserSummary(user.Id, user.Username)).ToArray()
                };
            }
        }

        public CreateTeamResult CreateTeam(int huntId, int userId, string teamName, string teamPassword)
        {
            if (string.IsNullOrWhiteSpace(teamName) ||
                teamName.Length != teamName.Trim().Length ||
                teamName.Any(c => !_WhitedlistedCharacters.Contains(c)))
                return CreateTeamResult.InvalidTeamName;

            var selectedHunt = (from hunt in Current.DB.Hunts
                                where hunt.Id == huntId
                                select hunt).FirstOrDefault();

            if (selectedHunt == null) return CreateTeamResult.HuntDoesNotExist;
            if (selectedHunt.Teams.Any(team => team.Name == teamName)) return CreateTeamResult.DuplicateName;

            var selectedUser = (from user in Current.DB.Users
                                where user.Id == userId
                                select user).FirstOrDefault();

            if (selectedUser == null) return CreateTeamResult.UserDoesNotExist;

            IEnumerable<int> teamsInHunt = from team in Current.DB.Teams
                                            where team.HuntId == huntId
                                            select team.Id;
            bool userIsInTeam = (from membership in Current.DB.TeamMemberships
                                    where membership.UserId == userId
                                    where teamsInHunt.Contains(membership.TeamId)
                                    select membership).Any();
            if (userIsInTeam) return CreateTeamResult.PlayerInTeam;

            Team newTeam = new Team
            {
                HuntId = huntId,
                Name = teamName,
                Password = BCrypt.Net.BCrypt.HashPassword(teamPassword, PasswordHashWorkFactor)
            };
            Current.DB.Teams.InsertOnSubmit(newTeam);
            Current.DB.SubmitChanges();
            TeamMembership teamMembership = new TeamMembership {TeamId = newTeam.Id, UserId = userId};
            newTeam.TeamMemberships.Add(teamMembership);
            Current.DB.SubmitChanges();
            return CreateTeamResult.Success;
        }

        public JoinTeamResult JoinTeam(int teamId, int userId, string teamPassword)
        {
            var team = (from t in Current.DB.Teams
                        where t.Id == teamId
                        select t).SingleOrDefault();
            if (team == null) return JoinTeamResult.TeamDoesNotExist;
            var user = (from u in Current.DB.Users
                        where u.Id == userId
                        select u).SingleOrDefault();
            if (user == null) return JoinTeamResult.UserDoesNotExist;
            if (team.TeamMemberships.Count >= 2) return JoinTeamResult.TeamFull;
            if (user.TeamMemberships.Where(tm => tm.Team.HuntId == team.HuntId).Any())
                return JoinTeamResult.UserInTeam;

            if (!BCrypt.Net.BCrypt.Verify(teamPassword, team.Password))
                return JoinTeamResult.PasswordIncorrect;

            TeamMembership membership = new TeamMembership {TeamId = teamId, UserId = userId};
            Current.DB.TeamMemberships.InsertOnSubmit(membership);
            Current.DB.SubmitChanges();
            return JoinTeamResult.Success;
        }
    }
}