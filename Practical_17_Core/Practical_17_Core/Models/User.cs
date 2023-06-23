using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Practical_17_Core.Models
{
    public class User: IdentityUser
    {
        [Required]
        [Column(TypeName = "VARCHAR(50)")]
        public string? FirstName { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(50)")]
        public string? LastName { get; set; }

        public bool IsDeleted { get; set; } = false;

        [NotMapped]
        public IEnumerable<string>? Claims { get; set; }
    }
}
