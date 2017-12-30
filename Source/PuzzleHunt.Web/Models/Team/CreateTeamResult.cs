namespace PuzzleHunt.Web.Models
{
    public enum CreateTeamResult
    {
        Success,
        HuntDoesNotExist,
        PlayerInTeam,
        DuplicateName,
        UserDoesNotExist,
        InvalidTeamName
    }

    public enum JoinTeamResult
    {
        Success,
        TeamDoesNotExist,
        UserInTeam,
        TeamFull,
        UserDoesNotExist,
        PasswordIncorrect
    }

    public enum JoinTeamEligibility
    {
        OK,
        TeamDoesNotExist,
        UserInTeam,
        TeamFull,
        UserDoesNotExist
    }
}