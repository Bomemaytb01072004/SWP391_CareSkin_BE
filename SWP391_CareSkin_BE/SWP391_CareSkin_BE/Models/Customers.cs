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

        public virtual ICollection<BlogNews> BlogNews { get; set; } = new List<BlogNews>();

        public virtual ICollection<Historys> Historys { get; set; } = new List<Historys>();

        public virtual ICollection<Results> Results { get; set; } = new List<Results>();

        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

        public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();

        public virtual ICollection<RatingFeedbacks> RatingFeedbacks { get; set; } = new List<RatingFeedbacks>();

        public virtual ICollection<Supports> Supports { get; set; } = new List<Supports>();


    }
}
