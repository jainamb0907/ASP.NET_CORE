using System.ComponentModel.DataAnnotations;

namespace Practical20.Models
{
    public class Students
    {



        [Key]
        public int? StudentId { get; set; }



        [Required]
        public string StudentName { get; set; } = string.Empty;



        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;



        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string Phone { get; set; } = string.Empty;
    }
}
