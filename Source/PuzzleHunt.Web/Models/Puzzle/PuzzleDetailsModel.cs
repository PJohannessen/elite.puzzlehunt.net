using System.Collections.Generic;

namespace PuzzleHunt.Web.Models
{
    public class PuzzleDetailsModel
    {
        public PuzzleModel Puzzle { get; private set; }
        public PuzzleStatus Status { get; private set; }
        public IList<HintModel> Hints { get; private set; }
        public IList<Completion> Completions { get; private set; }

        public PuzzleDetailsModel(PuzzleStatus status, PuzzleModel puzzle, IList<HintModel> hints, IList<Completion> completions)
        {
            Status = status;
            Puzzle = puzzle;
            Hints = hints ?? new List<HintModel>();
            Completions = completions;
        }
    }
}