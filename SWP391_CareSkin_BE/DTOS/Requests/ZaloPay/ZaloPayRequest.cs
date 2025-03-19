using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SWP391_CareSkin_BE.DTOs.Requests.ZaloPay
{
    public class ZaloPayRequest
    {
        public int OrderId { get; set; }
        public long Amount { get; set; }
    }
}
