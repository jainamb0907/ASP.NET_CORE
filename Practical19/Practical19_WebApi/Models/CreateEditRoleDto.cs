using System.ComponentModel.DataAnnotations;

namespace Practical19_WebApi.Models;

public class CreateEditRoleDto
{
    [Required]
    public string? RoleName { get; set; }
}
