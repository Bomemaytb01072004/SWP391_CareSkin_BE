using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    public class ZaloPayOrder
    {
        public int ZaloPayOrderId { get; set; }
        public long AppId { get; set; }
        public string AppUser { get; set; }
        public string AppTransId { get; set; }
        public long AppTime { get; set; }
        public long Amount { get; set; }
        public string Description { get; set; }
        public string Items { get; set; }
        public string EmbedData { get; set; }
        public string BankCode { get; set; }
        public string Mac { get; set; }
        public int OrderId { get; set; }
    }
}
