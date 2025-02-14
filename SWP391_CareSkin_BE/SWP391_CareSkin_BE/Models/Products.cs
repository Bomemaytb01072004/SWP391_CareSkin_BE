using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Product")]
    public class Products
    {
        [Key]
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int BrandId { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string Main_Infredients { get; set; }

        public int ML {  get; set; }

        public virtual Brands Brands { get; set; }
        public virtual ICollection<Carts> Carts { get; set; } = new List<Carts>();
        public virtual ICollection<SkinCareRoutineProducts> SkinCareRoutineProducts { get; set; } = new List<SkinCareRoutineProducts>();
        public virtual ICollection<PromotionProducts> PromotionProducts { get; set; } = new List<PromotionProducts>();
        public virtual ICollection<OrderProducts> OrderProducts { get; set; } = new List<OrderProducts>();
        public virtual ICollection<RatingFeedbacks> RatingFeedbacks { get; set; } = new List<RatingFeedbacks>();
    }
}
