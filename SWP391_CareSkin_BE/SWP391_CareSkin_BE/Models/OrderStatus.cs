using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("OrderStatus")]
    public class OrderStatus
    {
        public int OrderStatusId { get; set; }

        public string Status { get; set; }

        public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
    }
}
