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
        /// Handles the IPN (Instant Payment Notification) from Momo
        /// </summary>
        [HttpPost("momo_ipn")]
        public async Task<IActionResult> MomoIpn([FromBody] MomoCallbackDto callbackDto)
        {
            try
            {
                _logger.LogInformation("Received Momo IPN: {OrderId}, ResultCode: {ResultCode}", callbackDto.OrderId, callbackDto.ResultCode);
                
                if (callbackDto == null)
                {
                    _logger.LogWarning("Momo IPN received with null data");
                    return NoContent(); // Return 204 as required by Momo
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Momo IPN received with invalid model state");
                    return NoContent(); // Return 204 as required by Momo
                }

                bool isValid = _momoService.ValidateMomoCallback(callbackDto);
                if (!isValid)
                {
                    _logger.LogWarning("Momo IPN received with invalid signature");
                    return NoContent(); // Return 204 as required by Momo
                }

                // Process the IPN asynchronously to ensure we respond quickly
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _momoService.HandleMomoCallbackAsync(callbackDto);
                        _logger.LogInformation("Successfully processed Momo IPN for order {OrderId}", callbackDto.OrderId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing Momo IPN for order {OrderId}", callbackDto.OrderId);
                    }
                });

                // Momo expects a 204 No Content response within 15 seconds
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception but still return 204 No Content to Momo
                _logger.LogError(ex, "Error handling Momo IPN: {Message}", ex.Message);
                return NoContent();
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
