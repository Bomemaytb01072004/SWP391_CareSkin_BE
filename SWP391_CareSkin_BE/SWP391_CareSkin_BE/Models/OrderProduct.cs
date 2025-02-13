using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("OrderProduct")]
    public class OrderProduct
    {
        public int OrderProductId { get; set; }
        public int Order_ID { get; set; }

        public int Product_ID { get; set; }

        public int Quantity { get; set; }

        public virtual Product Product { get; set; }

        public virtual Orders Orders { get; set; }
    }
}
