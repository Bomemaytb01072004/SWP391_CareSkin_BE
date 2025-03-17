using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("MomoPayment")]
    public class MomoPayment
    {
        [Key]
        public string MomoPaymentId { get; set; } = string.Empty;
        
        public int OrderId { get; set; }
        
        public long Amount { get; set; }
        
        public string OrderInfo { get; set; } = string.Empty;
        
        public bool IsPaid { get; set; }
        
        public bool IsExpired { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? PaymentTime { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } = null!;
    }
}
