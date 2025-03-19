using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{

    [Table("Admin")]
    public class Admin
    {
        public int AdminId {  get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateOnly? DoB {  get; set; }

        public string PictureUrl { get; set; }
        [NotMapped]
        public string? Token { get; set; }

        [NotMapped]
        public string Role { get; set; }

        public virtual ICollection<BlogNews>? BlogNews { get; set; } = new List<BlogNews>();
        public Admin()
        {
            BlogNews = new HashSet<BlogNews>();//
        }
    }
}
