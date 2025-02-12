using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Customers")]
    public class Customers
    {
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(20)]
        public string Password { get; set; }

        public string FullName { get; set; }

        [Required, MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$")]
        public string Email { get; set; }

        public int Phone { get; set; }

        public string Dob { get; set; }

        public string Gender { get; set; }

        public string ProfilePicture { get; set; }

        public string Address { get; set; }

        public int Points { get; set; }
    }
}
