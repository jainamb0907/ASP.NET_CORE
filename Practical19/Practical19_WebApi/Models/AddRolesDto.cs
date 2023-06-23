using System.ComponentModel.DataAnnotations;

namespace Practical19_WebApi.Models;

public class AddRolesDto
{
    [Required]
    public IEnumerable<string>? Roles { get; set; }
}
