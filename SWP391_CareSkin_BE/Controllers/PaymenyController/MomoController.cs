using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOs.Responses;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers.PaymenyController
{
    [Route("api/[controller]")]
    [ApiController]
    public class MomoController : ControllerBase
    {

        private readonly IMomoService _momoService;
        private readonly IMomoRepository _paymentRepository;

        public MomoController(IMomoService momoService, IMomoRepository paymentRepository)
        {
            _momoService = momoService;
            _paymentRepository = paymentRepository;
        }

        /// <summary>
        /// Tạo yêu cầu thanh toán MoMo
        /// </summary>
        [HttpPost("create-payment")]
        public IActionResult CreatePayment([FromBody] MomoRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Invalid payment request." });
            }

            string payUrl = _momoService.CreatePayment(request);

            if (string.IsNullOrEmpty(payUrl))
            {
                return BadRequest(new { message = "Failed to create MoMo payment." });
            }

            return Ok(new { payUrl });
        }

        /// <summary>
        /// Xử lý phản hồi từ MoMo (callback URL)
        /// </summary>
        [HttpPost("momo-callback")]
        public IActionResult MomoCallback([FromBody] MomoResponse response)
        {
            if (response == null)
            {
                return BadRequest(new { message = "Invalid MoMo response." });
            }

            var transaction = _paymentRepository.GetTransactionByOrderId(response.OrderId);

            if (transaction == null)
            {
                return NotFound(new { message = "Transaction not found." });
            }

            // Cập nhật trạng thái thanh toán
            transaction.Status = response.ResultCode == 0 ? "Success" : "Failed";
            _paymentRepository.SavePaymentTransaction(transaction);

            return Ok(new { message = "Payment status updated.", status = transaction.Status });
        }
    }
}
