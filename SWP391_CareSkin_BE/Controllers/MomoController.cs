using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOS.Requests.Momo;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Text.Json;

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
                    var errorResponse = _momoService.CreateErrorResponse("Bad format request.", 0);
                    return BadRequest(errorResponse);
                }

                var response = await _momoService.CreateMomoPaymentAsync(dto);

                if (response.ErrorCode != 0)
                {
                    return BadRequest(response);
                }

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
        /// Receives data from frontend in JSON format
        /// </summary>
        [HttpPost("momo_ipn")]
        public async Task<IActionResult> MomoIpn([FromBody] MomoCallbackDto callbackDto)
        {
            try
            {
                if (callbackDto == null)
                {
                    _logger.LogWarning("Received null Momo callback data");
                    return BadRequest(new { message = "Invalid callback data" });
                }

                _logger.LogInformation("Received Momo callback: OrderId={OrderId}, ResultCode={ResultCode}, TransId={TransId}",
                    callbackDto.OrderId, callbackDto.ResultCode, callbackDto.TransId);

                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                    _logger.LogWarning("Momo callback received with invalid model state: {Errors}", errors);
                    return BadRequest(new { message = "Invalid model state", errors });
                }

                // Process the callback asynchronously
                await ProcessMomoCallbackAsync(callbackDto, "Frontend");

                return Ok(new { success = true, message = "Callback processed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling Momo callback: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = "Internal server error" });
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

        /// <summary>
        /// Helper method to process Momo callbacks asynchronously with proper scoping
        /// </summary>
        private Task ProcessMomoCallbackAsync(MomoCallbackDto callbackDto, string source)
        {
            var serviceProvider = HttpContext.RequestServices;
            
            return Task.Run(async () =>
            {
                // Create a new scope to get a fresh instance of the DbContext
                using (var scope = serviceProvider.CreateScope())
                {
                    try
                    {
                        var scopedMomoService = scope.ServiceProvider.GetRequiredService<IMomoService>();
                        var scopedLogger = scope.ServiceProvider.GetRequiredService<ILogger<MomoController>>();
                        
                        await scopedMomoService.HandleMomoCallbackAsync(callbackDto);
                        scopedLogger.LogInformation("Successfully processed Momo {Source} for order {OrderId}", source, callbackDto.OrderId);
                    }
                    catch (Exception ex)
                    {
                        var scopedLogger = scope.ServiceProvider.GetRequiredService<ILogger<MomoController>>();
                        scopedLogger.LogError(ex, "Error processing Momo {Source} for order {OrderId}: {Message}", 
                            source, callbackDto.OrderId, ex.Message);
                    }
                }
            });
        }
    }
}
