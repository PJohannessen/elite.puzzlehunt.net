using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PuzzleHunt.Web.Models
{
    public class EditPuzzleModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public string Difficulty { get; set; }

        [Required]
        public string Answer { get; set; }

        [Required]
        [AllowHtml]
        public string Content { get; set; }

        [Required]
        [AllowHtml]
        public string Solution { get; set; }
    }
}