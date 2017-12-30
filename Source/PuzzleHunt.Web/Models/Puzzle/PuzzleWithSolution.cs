namespace PuzzleHunt.Web.Models
{
    public class PuzzleWithSolution
    {
        public string Name { get; private set; }
        public string Solution { get; private set; }

        public PuzzleWithSolution(string name, string solution)
        {
            Name = name;
            Solution = solution;
        }
    }
}