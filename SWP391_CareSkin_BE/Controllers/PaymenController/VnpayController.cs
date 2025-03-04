using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOs.Requests.Vnpay;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [ApiController]
    [Route("api/vnpay")]
    public class VnpayController : ControllerBase
    {
        private readonly IVnpayService _vnpayService;

        public VnpayController(IVnpayService vnpayService)
        {
            _vnpayService = vnpayService;
        }

        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] VnpayRequestDTO request)
        {
            var response = await _vnpayService.CreatePaymentAsync(request);
            return Ok(response);
        }

        [HttpGet("payment-callback")]
        public async Task<IActionResult> PaymentCallback()
        {
            var queryParams = Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString());

            bool result = await _vnpayService.ProcessPaymentCallbackAsync(queryParams);
            if (!result)
            {
                return BadRequest(new { message = "Thanh toán không hợp lệ hoặc thất bại" });
            }

            return Ok(new { message = "Thanh toán thành công" });
        }

    }
}
