using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PuzzleHunt.Web.Models
{
    public class CreateTeamModel
    {
        [Required]
        [Display(Name = "Name")]
        [StringLength(32)]
        public string Name { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}