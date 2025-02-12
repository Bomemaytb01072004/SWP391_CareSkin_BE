using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{

    [Table("Admin")]
    public class Admin
    {
        [Key]
        public int Id {  get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public int Phone { get; set; }

        public DateTime DoB {  get; set; }

        public string Profile_Picture { get; set; }





    }
}
