using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Order")]
    public class Order
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public int OrderStatusId { get; set; }
   
        public int? PromotionId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        public decimal TotalPriceSale { get; set; }
        public DateOnly OrderDate { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual OrderStatus OrderStatus { get; set; }

        public virtual Promotion? Promotion { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

        public virtual ICollection<VnpayTransactions>? VnpayTransactions { get; set; } = new List<VnpayTransactions>();
    }
}
