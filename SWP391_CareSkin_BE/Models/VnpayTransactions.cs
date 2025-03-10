using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("VnpayTransactions")]
    public class VnpayTransactions
    {
        [Key]
        public int TransactionId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string OrderDescription { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; } // "Pending", "Success", "Failed"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("OrderId")]
        public virtual Order order { get; set; }
    }
}
