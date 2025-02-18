using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("ProductDetailIngredient")]
    public class ProductDetailIngredient
    {
        public int ProductDetailIngredientId { get; set; }

        public int ProductId { get; set; }

        public string IngredientName { get; set; }

        public virtual Product Product { get; set; }
    }
}
