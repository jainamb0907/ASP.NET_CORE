using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Practical_17_Core.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 3)]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 3)]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email address is invalid")]
        [StringLength(200, MinimumLength = 10)]
        [Remote(action: "IsEmailInUse", controller: "Users")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
