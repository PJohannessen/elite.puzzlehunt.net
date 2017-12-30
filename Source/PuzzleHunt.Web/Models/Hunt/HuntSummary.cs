namespace PuzzleHunt.Web.Models
{
    public class HuntSummary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserSummary Creator { get; set; }
    }
}