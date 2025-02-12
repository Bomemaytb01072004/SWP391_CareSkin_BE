using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int Product_Id { get; set; }

        public string ProductName { get; set; }

        public int Brand_ID { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string Main_Infredients { get; set; }

        public int ML {  get; set; }

        public virtual Brand Brands { get; set; }
        public virtual ICollection<SkinCareRoutineProduct> SkinCareRoutineProducts { get; set; } = new List<SkinCareRoutineProduct>();
        public virtual ICollection<PromotionProduct> PromotionProducts { get; set; } = new List<PromotionProduct>();
    }
}
