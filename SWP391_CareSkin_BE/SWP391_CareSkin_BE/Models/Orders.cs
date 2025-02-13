using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Orders")]
    public class Orders
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public string OrderStatusId { get; set; }

        public int TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public virtual Customers Customers { get; set; }

        public virtual OrderStatus OrderStatus { get; set; }

        public virtual PromotionOrder PromotionOrder { get; set; }

        public virtual OrderProduct OrderProduct { get; set; }
    }
}
