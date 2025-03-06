using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Staff")]
    public class Staff
    {
        public int StaffId { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Phone {  get; set; }

        public DateOnly? DoB {  get; set; }

        public string? PictureUrl { get; set; }
        [NotMapped]
        public string? Token { get; set; }

        [NotMapped]
        public string Role { get; set; }

        public virtual ICollection<Support> Supports { get; set; } = new List<Support>();

    }
}
