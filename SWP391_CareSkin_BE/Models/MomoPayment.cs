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
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        
        public string OrderInfo { get; set; } = string.Empty;
        
        public bool IsPaid { get; set; }
        
        public bool IsExpired { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? PaymentTime { get; set; }
        
        public string? TransactionId { get; set; }
        
        public string? PaymentMethod { get; set; }
        
        public string? Status { get; set; }
        
        public string? ResponseMessage { get; set; }
        
        public string? ResponseCode { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } = null!;
    }
}
