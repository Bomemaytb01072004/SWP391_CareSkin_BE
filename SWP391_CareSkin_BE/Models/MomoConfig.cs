using System;

namespace SWP391_CareSkin_BE.Models
{
    public class MomoConfig
    {
        public string MomoApiUrl { get; set; } = string.Empty;
        public string PartnerCode { get; set; } = string.Empty;
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
        public string IpnUrl { get; set; } = string.Empty;
        public string RequestType { get; set; } = string.Empty;
    }
}
