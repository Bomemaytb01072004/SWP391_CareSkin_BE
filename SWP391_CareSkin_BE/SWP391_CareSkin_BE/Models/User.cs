using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int IdUser { get; set; }

        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(20)]
        public string Password { get; set; }

        [Required, MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$")]
        public string Email { get; set; }
    }
}
