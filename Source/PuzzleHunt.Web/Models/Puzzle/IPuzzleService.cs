namespace PuzzleHunt.Web.Models
{
    public interface IPuzzleService
    {
        CreatePuzzleResult Create(int huntId, int userId, CreatePuzzleModel model);
        EditPuzzleResult Edit(int userId, EditPuzzleModel model);
        AnswerPuzzleResult Answer(int puzzleId, int userId, string answer);
        PuzzleDetailsModel GetPuzzleDetailsAndStatus(int puzzleId, int userId);
        EditPuzzleDetails GetPuzzleForEdit(int puzzleId, int userId);
        UnlockHintResult UnlockHint(int puzzleId, int userId);
        AdminDetails GetAdminDetails(int userId);
        PuzzleSolutionModel GetPuzzleSolution(int puzzleId);
    }
}
