using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("ProductMainIngredient")]
    public class ProductMainIngredient
    {
        public int ProductMainIngredientId { get; set; }

        public int ProductId { get; set; }

        public string IngredientName { get; set; }

        public string Description { get; set; }

        public virtual Product Product { get; set; }
    }
}
