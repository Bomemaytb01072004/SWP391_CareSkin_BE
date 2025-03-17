using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOS.Requests.Momo;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/momo")]
    [ApiController]
    public class MomoController : ControllerBase
    {
        private readonly IMomoService _momoService;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<MomoController> _logger;

        public MomoController(IMomoService momoService, IOrderRepository orderRepository, ILogger<MomoController> logger)
        {
            _momoService = momoService;
            _orderRepository = orderRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a Momo payment request
        /// </summary>
        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] MomoPaymentRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Let the service handle the error response format
                    var errorResponse = _momoService.CreateErrorResponse("Bad format request.", 0);
                    return BadRequest(errorResponse);
                }

                var response = await _momoService.CreateMomoPaymentAsync(dto);

                if (response.ErrorCode != 0)
                {
                    // The service already formatted the error response
                    return BadRequest(response);
                }

                // Return successful response
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Momo payment for order {OrderId}", dto.OrderId);
                var errorResponse = _momoService.CreateErrorResponse("Bad format request.", 0);
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        /// Handles the callback from Momo (notifyUrl)
        /// </summary>
        [HttpPost("notify")]
        public async Task<IActionResult> MomoNotify([FromBody] MomoCallbackDto callbackDto)
        {
            try
            {
                if (callbackDto == null)
                {
                    return BadRequest(new { message = "Callback data is required" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool isValid = _momoService.ValidateMomoCallback(callbackDto);
                if (!isValid)
                {
                    return BadRequest(new { message = "Invalid signature" });
                }

                await _momoService.HandleMomoCallbackAsync(callbackDto);

                // Momo expects a 200 OK response
                return Ok(new { message = "Callback handled successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception but still return 200 OK to Momo
                Console.WriteLine($"Error handling Momo callback: {ex.Message}");
                return Ok(new { message = "Callback received" });
            }
        }

        /// <summary>
        /// Handles the return URL callback from Momo (returnUrl)
        /// </summary>
        [HttpGet("return")]
        public async Task<IActionResult> MomoReturn([FromQuery] string orderId)
        {
            try
            {
                int orderIdInt;
                if (!int.TryParse(orderId, out orderIdInt))
                {
                    return BadRequest(new { success = false, message = "Invalid order ID" });
                }

                // Check payment status
                var paymentStatus = await _momoService.CheckPaymentStatusAsync(orderIdInt);

                // Return payment status to client
                return Ok(new
                {
                    success = true,
                    isPaid = paymentStatus.IsPaid,
                    paymentTime = paymentStatus.PaymentTime
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Checks the payment status of an order
        /// </summary>
        [HttpGet("check-payment/{orderId}")]
        public async Task<IActionResult> CheckPayment(int orderId)
        {
            try
            {
                var paymentStatus = await _momoService.CheckPaymentStatusAsync(orderId);

                return Ok(new
                {
                    success = true,
                    isPaid = paymentStatus.IsPaid,
                    orderId = orderId,
                    amount = paymentStatus.Amount,
                    paymentTime = paymentStatus.PaymentTime
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
