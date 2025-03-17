using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOS.Requests.Momo;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

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
        /// </summary>
        [HttpPost("momo_ipn")]
        public async Task<IActionResult> MomoIpn([FromBody] MomoCallbackDto callbackDto)
        {
            try
            {
                _logger.LogInformation("Received Momo IPN: {OrderId}, ResultCode: {ResultCode}", callbackDto.OrderId, callbackDto.ResultCode);
                
                if (callbackDto == null || !ModelState.IsValid)
                {
                    _logger.LogWarning("Momo IPN received with null data or invalid model state");
                    return NoContent(); // Return 204 as required by Momo
                }

                bool isValid = _momoService.ValidateMomoCallback(callbackDto);
                if (!isValid)
                {
                    _logger.LogWarning("Momo IPN received with invalid signature");
                    return NoContent();
                }

                // Process the IPN asynchronously to ensure we respond quickly
                _ = ProcessMomoCallbackAsync(callbackDto, "IPN");

                // Momo expects a 204 No Content response within 15 seconds
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling Momo IPN: {Message}", ex.Message);
                return NoContent();
            }
        }

        /// <summary>
        /// Handles the redirect from Momo with query parameters for both success and failure cases
        /// </summary>
        [HttpGet("redirect")]
        public IActionResult MomoRedirect()
        {
            try
            {
                // Get query parameters directly from the request
                var query = HttpContext.Request.Query;
                
                string orderId = query["orderId"].ToString();
                string resultCode = query["resultCode"].ToString();
                
                _logger.LogInformation("Received Momo redirect: OrderId={OrderId}, ResultCode={ResultCode}", orderId, resultCode);
                
                // Create a callback DTO from query parameters
                var callbackDto = new MomoCallbackDto
                {
                    PartnerCode = query["partnerCode"].ToString() ?? string.Empty,
                    OrderId = orderId ?? string.Empty,
                    RequestId = query["requestId"].ToString() ?? string.Empty,
                    Amount = !string.IsNullOrEmpty(query["amount"].ToString()) ? long.Parse(query["amount"].ToString()) : 0,
                    OrderInfo = query["orderInfo"].ToString() ?? string.Empty,
                    OrderType = query["orderType"].ToString() ?? string.Empty,
                    TransId = !string.IsNullOrEmpty(query["transId"].ToString()) ? long.Parse(query["transId"].ToString()) : 0,
                    ResultCode = !string.IsNullOrEmpty(resultCode) ? int.Parse(resultCode) : 1006,
                    Message = query["message"].ToString() ?? string.Empty,
                    PayType = query["payType"].ToString() ?? string.Empty,
                    ResponseTime = !string.IsNullOrEmpty(query["responseTime"].ToString()) ? long.Parse(query["responseTime"].ToString()) : 0,
                    ExtraData = query["extraData"].ToString() ?? string.Empty,
                    Signature = query["signature"].ToString() ?? string.Empty
                };

                // Process the callback asynchronously
                _ = ProcessMomoCallbackAsync(callbackDto, "Redirect");

                // Redirect to the frontend with appropriate parameters
                string redirectUrl = $"http://careskinbeauty.shop/?orderId={orderId}&resultCode={resultCode}";
                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling Momo redirect: {Message}", ex.Message);
                return Redirect("http://careskinbeauty.shop/?error=true");
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
                        scopedLogger.LogError(ex, "Error processing Momo {Source} for order {OrderId}", source, callbackDto.OrderId);
                    }
                }
            });
        }
    }
}
