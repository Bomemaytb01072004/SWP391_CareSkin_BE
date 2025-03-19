using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    public class ZaloPayRedirect
    {
        public int ZaloPayRedirectId { get; set; }
        public string AppId { get; set; }
        public string AppTransId { get; set; }
        public string Pmcid { get; set; }
        public string BankCode { get; set; }
        public long Amount { get; set; }
        public string DiscountAmount { get; set; }
        public string Status { get; set; }
        public string Checksum { get; set; }
    }
}
