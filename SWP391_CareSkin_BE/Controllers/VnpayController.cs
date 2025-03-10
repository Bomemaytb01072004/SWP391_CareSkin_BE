using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOs.Requests.Vnpay;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [ApiController]
    [Route("api/vnpay")]
    public class VnpayController : ControllerBase
    {
        private readonly IVnpayService _vnPayService;

        public VnpayController(IVnpayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        // Tạo thanh toán
        [HttpPost("create-payment")]
        public IActionResult CreatePayment([FromBody] VnpayRequestDTO model)
        {
            var paymentUrl = _vnPayService.CreatePaymentUrl(model, HttpContext);
            return Ok(new { paymentUrl });
        }


        // Nhận callback từ VNPAY
        [HttpGet("callback")]
        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }
    }
}
