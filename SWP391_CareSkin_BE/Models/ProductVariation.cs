using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SWP391_CareSkin_BE.Models
{
    [Table("ProductVariation")]
    public class ProductVariation
    {
        public int ProductVariationId { get; set; }

        public int ProductId { get; set; }

        public int Ml { get; set; }

        public double Price { get; set; }

        public virtual Product Product { get; set; }

        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

        public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();    
    }
}
