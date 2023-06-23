using System.ComponentModel.DataAnnotations;

namespace Practical18.Model
{
    public class StudentViewModel
    {
        [Required]
        [StringLength(50)]
        public string StudentName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(10, MinimumLength = 10)]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }
    }
}
