using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("MomoCallback")]
    public class MomoCallback
    {
        [Key]
        public int MomoCallbackId { get; set; }
        
        public string PartnerCode { get; set; } = string.Empty;
        
        public string OrderId { get; set; } = string.Empty;
        
        public string RequestId { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        
        public string OrderInfo { get; set; } = string.Empty;
        
        public string OrderType { get; set; } = string.Empty;
        
        public long TransId { get; set; }
        
        public int ResultCode { get; set; }
        
        public string Message { get; set; } = string.Empty;
        
        public string PayType { get; set; } = string.Empty;
        
        public long ResponseTime { get; set; }
        
        public string ExtraData { get; set; } = string.Empty;
        
        public string Signature { get; set; } = string.Empty;
        
        public DateTime ReceivedDate { get; set; }
    }
}
