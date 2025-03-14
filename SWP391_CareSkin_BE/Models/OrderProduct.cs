    using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("OrderProduct")]
    public class OrderProduct
    {
        public int OrderProductId { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int ProductVariationId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }

        public virtual Product Product { get; set; }

        public virtual Order Order { get; set; }

        public virtual ProductVariation ProductVariation { get; set; }
    }
}
    