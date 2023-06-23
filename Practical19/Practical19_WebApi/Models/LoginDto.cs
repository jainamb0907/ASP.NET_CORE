using System.ComponentModel.DataAnnotations;

namespace Practical19_WebApi.Models;

public class LoginDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Email address is invalid")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}
