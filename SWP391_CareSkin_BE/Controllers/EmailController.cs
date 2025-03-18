using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOs.Requests.Email;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<EmailController> _logger;

        public EmailController(
            IEmailService emailService, 
            IOrderRepository _orderRepository,
            ILogger<EmailController> logger)
        {
            _emailService = emailService;
            this._orderRepository = _orderRepository;
            _logger = logger;
        }

        /// <summary>
        /// Sends an order confirmation email
        /// </summary>
        [HttpPost("send-order-confirmation")]
        public async Task<IActionResult> SendOrderConfirmation([FromBody] OrderEmailRequestDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verify that the order exists
                var order = await _orderRepository.GetOrderByIdAsync(request.OrderId);
                if (order == null)
                {
                    return NotFound(new { message = $"Order with ID {request.OrderId} not found" });
                }

                // Send the email
                await _emailService.SendOrderConfirmationEmailAsync(
                    request.Email,
                    request.OrderId.ToString(),
                    request.CustomerName,
                    order.TotalPriceSale);

                return Ok(new { message = "Order confirmation email sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending order confirmation email for order {OrderId}", request.OrderId);
                return StatusCode(500, new { message = "An error occurred while sending the email", error = ex.Message });
            }
        }

        /// <summary>
        /// Sends a payment confirmation email
        /// </summary>
        [HttpPost("send-payment-confirmation")]
        public async Task<IActionResult> SendPaymentConfirmation([FromBody] PaymentEmailRequestDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verify that the order exists
                var order = await _orderRepository.GetOrderByIdAsync(request.OrderId);
                if (order == null)
                {
                    return NotFound(new { message = $"Order with ID {request.OrderId} not found" });
                }

                // Send the email
                await _emailService.SendPaymentConfirmationEmailAsync(
                    request.Email,
                    request.OrderId.ToString(),
                    request.CustomerName,
                    request.PaymentAmount,
                    request.PaymentMethod);

                return Ok(new { message = "Payment confirmation email sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending payment confirmation email for order {OrderId}", request.OrderId);
                return StatusCode(500, new { message = "An error occurred while sending the email", error = ex.Message });
            }
        }

        /// <summary>
        /// Sends a custom email
        /// </summary>
        [HttpPost("send-custom-email")]
        public async Task<IActionResult> SendCustomEmail([FromBody] CustomEmailRequestDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Send the email
                await _emailService.SendEmailAsync(
                    request.Email,
                    request.Subject,
                    request.Body);

                return Ok(new { message = "Email sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending custom email to {Email}", request.Email);
                return StatusCode(500, new { message = "An error occurred while sending the email", error = ex.Message });
            }
        }
    }
}
