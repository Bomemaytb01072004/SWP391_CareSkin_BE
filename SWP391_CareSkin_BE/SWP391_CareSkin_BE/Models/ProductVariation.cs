using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("ProductVariation")]
    public class ProductVariation
    {
        public int ProductVariationId { get; set; }

        public int ProductId { get; set; }

        public int Ml { get; set; }

        public int price { get; set; }

        public virtual Product Product { get; set; }
    }
}
