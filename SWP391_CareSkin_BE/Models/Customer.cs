using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Customers")]
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        public string? FullName { get; set; }

        [Required, MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$")]
        public string Email { get; set; }

        public string? Phone { get; set; }

        public DateOnly? Dob { get; set; }

        public string? Gender { get; set; }

        public string? PictureUrl { get; set; }

        public string? Address { get; set; }
        [NotMapped] 
        public string? Token { get; set; }

        public virtual ICollection<BlogNew>? BlogNews { get; set; } = new List<BlogNew>();

        public virtual ICollection<History>? Historys { get; set; } = new List<History>();

        public virtual ICollection<Result>? Results { get; set; } = new List<Result>();

        public virtual ICollection<Cart>? Carts { get; set; } = new List<Cart>();

        public virtual ICollection<Order>? Orders { get; set; } = new List<Order>();

        public virtual ICollection<RatingFeedback>? RatingFeedbacks { get; set; } = new List<RatingFeedback>();

        public virtual ICollection<Support>? Supports { get; set; } = new List<Support>();

        public virtual ICollection<PromotionCustomer>? PromotionCustomers { get; set; } = new List<PromotionCustomer>();
    }
}
