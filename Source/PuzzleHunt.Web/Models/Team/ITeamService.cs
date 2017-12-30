using System.Collections.Generic;

namespace PuzzleHunt.Web.Models
{
    public interface ITeamService
    {
        TeamSummary GetTeam(int id);
        JoinTeamEligibility GetTeamEligibility(int id, int userId);
        IEnumerable<TeamSummary> GetTeams(int huntId);
        CreateTeamResult CreateTeam(int huntId, int userId, string teamName, string teamPassword);
        JoinTeamResult JoinTeam(int teamId, int userId, string teamPassword);
    }
}
