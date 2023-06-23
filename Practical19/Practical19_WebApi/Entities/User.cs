using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practical19_WebApi.Entities;

public class User : IdentityUser
{
    [Required]
    [Column(TypeName = "VARCHAR(50)")]
    public string? FirstName { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR(50)")]
    public string? LastName { get; set; }
}
