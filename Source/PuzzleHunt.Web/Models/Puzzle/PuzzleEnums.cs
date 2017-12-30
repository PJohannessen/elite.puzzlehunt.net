namespace PuzzleHunt.Web.Models
{
    public enum CreatePuzzleResult
    {
        Success,
        HuntDoesNotExist,
        UserNotAdmin
    }

    public enum EditPuzzleResult
    {
        Success,
        PuzzleDoesNotExist,
        UserNotAdmin
    }

    public enum AnswerPuzzleResult
    {
        Success,
        Incorrect,
        PuzzleDoesNotExist,
        UserNotInTeam,
        PuzzleAlreadyCompleted,
        NoPuzzleResultRecord
    }

    public enum PuzzleStatus
    {
        Completed,
        Started,
        HuntNotStarted,
        HuntFinished,
        PuzzleNotStarted,
        PuzzleDoesNotExist,
        UserNotInTeam,
        UserIsAdmin
    }

    public enum UserPuzzleStatus
    {
        Completed,
        Started,
        NotStarted,
        UserNotInTeam
    }

    public enum UnlockHintResult
    {
        Success,
        NoHintsRemaining,
        PuzzleDoesNotExist
    }
}