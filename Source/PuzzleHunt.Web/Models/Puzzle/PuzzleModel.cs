namespace PuzzleHunt.Web.Models
{
    public class PuzzleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserSummary Creator { get; set; }
        public string Difficulty { get; set; }
        public string Content { get; set; }

    }
}