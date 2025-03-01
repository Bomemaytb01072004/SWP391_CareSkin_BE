using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int BrandId { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public string PictureUrl { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public virtual ICollection<SkinCareRoutineProduct> SkinCareRoutineProducts { get; set; } = new List<SkinCareRoutineProduct>();
        public virtual ICollection<PromotionProduct> PromotionProducts { get; set; } = new List<PromotionProduct>();
        public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
        public virtual ICollection<RatingFeedback> RatingFeedbacks { get; set; } = new List<RatingFeedback>();
        public virtual ICollection<ProductVariation> ProductVariations { get; set; } = new List<ProductVariation>();
        public virtual ICollection<ProductMainIngredient> ProductMainIngredients { get; set; } = new List<ProductMainIngredient>();
        public virtual ICollection<ProductDetailIngredient> ProductDetailIngredients { get; set; } = new List<ProductDetailIngredient>();
        public virtual ICollection<ProductPicture> ProductPictures { get; set; } = new List<ProductPicture>();
        public virtual ICollection<ProductUsage> ProductUsages { get; set; } = new List<ProductUsage>();
        public virtual ICollection<ProductForSkinType> ProductForSkinTypes { get; set; } = new List<ProductForSkinType>();
    }
}
