using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Practical19_WebApi.Models;

public class RegisterDto
{
    [Required]
    public string? FirstName { get; set; }

    [Required]
    public string? LastName { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Email address is invalid")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8)]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
    public string? ConfirmPassword { get; set; }
}
