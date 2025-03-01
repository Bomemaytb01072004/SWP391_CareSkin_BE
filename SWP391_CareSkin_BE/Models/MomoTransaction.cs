using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("MomoTransactions")]
    public class MomoTransaction
    {
        [Key]
        public int Id { get; set; }
        public string OrderId { get; set; }
        public long Amount { get; set; }
        public string PayUrl { get; set; }
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
