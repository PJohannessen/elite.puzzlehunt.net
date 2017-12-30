namespace PuzzleHunt.Web.Models
{
    public class EditPuzzleDetails
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Content { get; private set; }
        public string Answer { get; private set; }
        public int Order { get; private set; }
        public string Difficulty { get; private set; }
        public string Solution { get; private set; }

        public EditPuzzleDetails(int id, string name, string content, string answer, int order, string difficulty, string solution)
        {
            Id = id;
            Name = name;
            Content = content;
            Answer = answer;
            Order = order;
            Difficulty = difficulty;
            Solution = solution;
        }
    }
}