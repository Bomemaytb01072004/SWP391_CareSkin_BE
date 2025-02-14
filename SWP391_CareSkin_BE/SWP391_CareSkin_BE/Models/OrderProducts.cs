using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("OrderProduct")]
    public class OrderProducts
    {
        public int OrderProductId { get; set; }
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public virtual Products Product { get; set; }

        public virtual Orders Orders { get; set; }
    }
}
