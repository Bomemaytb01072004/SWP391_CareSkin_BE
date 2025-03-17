using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("MomoCallback")]
    public class MomoCallback
    {
        [Key]
        public int MomoCallbackId { get; set; }
        
        public string RequestId { get; set; } = string.Empty;
        
        public string OrderId { get; set; } = string.Empty;
        
        public long Amount { get; set; }
        
        public int ResultCode { get; set; }
        
        public string Message { get; set; } = string.Empty;
        
        public string Signature { get; set; } = string.Empty;
        
        public DateTime ReceivedDate { get; set; }
    }
}
