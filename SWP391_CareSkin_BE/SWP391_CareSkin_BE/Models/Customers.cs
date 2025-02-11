using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Customers")]
    public class Customers
    {
        [Key]
        public int CustomerId { get; set; }

        public string UserName  { get; set; }

        public string PassWord { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public int Phone { get; set; }

        public string Dob { get; set; }

        public string Gender { get; set; }

        public string ProfilePicture { get; set; }

        public string Address { get; set; }

        public int Points { get; set; }
    }
}
