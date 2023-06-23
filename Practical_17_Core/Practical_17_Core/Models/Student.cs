using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;



namespace Practical_17_Core.Models
{
    public class Student
    {
        [Display(Name = "Student Id")]
        public Guid Id { get; set; } = Guid.NewGuid();


        [Display(Name = "First Name")]
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Column(TypeName = "VARCHAR(50)")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Column(TypeName = "VARCHAR(50)")]
        public string? LastName { get; set; }

        [Display(Name = "Email")]
        [Required]
        [StringLength(200, MinimumLength = 10)]
        [EmailAddress]
        [Column(TypeName = "VARCHAR(50)")]
        public string? Email { get; set; }

        [Display(Name = "Mobile Number")]
        [Required]
        [StringLength(10, MinimumLength = 10)]
        [Column(TypeName = "VARCHAR(10)")]
        [Phone]
        public string? MobileNumber { get; set; }

        [Display(Name = "Address")]
        [MaxLength(500)]
        [Column(TypeName = "VARCHAR(500)")]
        public string? Address { get; set; }
    }
}
