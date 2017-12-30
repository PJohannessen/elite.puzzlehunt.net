namespace PuzzleHunt.Web.Models
{
    public class PuzzleSolutionModel
    {
        public bool HuntIsOver { get; private set; }
        public PuzzleWithSolution Puzzle { get; private set; }

        public PuzzleSolutionModel(bool huntIsOver, PuzzleWithSolution puzzle)
        {
            HuntIsOver = huntIsOver;
            Puzzle = puzzle;
        }
    }
}