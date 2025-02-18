using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Order")]
    public class Order
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public int OrderStatusId { get; set; }

        public int TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual OrderStatus OrderStatus { get; set; }

        public virtual ICollection<PromotionOrder> PromotionOrders { get; set; } = new List<PromotionOrder>();
        public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}
