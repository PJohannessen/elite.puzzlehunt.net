using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PuzzleHunt.Web.Models
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "Username")]
        [StringLength(32)]
        public string Username { get; set; }

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