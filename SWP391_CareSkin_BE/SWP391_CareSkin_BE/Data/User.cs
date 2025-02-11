using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Data
{
    [Table("User")]
    public class User
    {
        [Key]
        public int IdUser { get; set; }

        [Required]
        [MaxLength(20)]
        public String UserName { get; set; }

        [Required]
        [MaxLength(20)]
        public String Password { get; set; }

        [Required, MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$")]
        public string Email { get; set; }
    }
}
