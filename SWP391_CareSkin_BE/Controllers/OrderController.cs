using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOs.Requests.Order;
using SWP391_CareSkin_BE.DTOs.Responses;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // POST: api/Order
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateRequestDTO request)
        {
            var createdOrder = await _orderService.CreateOrderAsync(request);
            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.OrderId }, createdOrder);
        }

        // GET: api/Order/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        // GET: api/Order/customer/{customerId}
        [HttpGet("customer/{customerId}")]
        
        public async Task<IActionResult> GetOrdersByCustomerId(int customerId)
        {
            var orders = await _orderService.GetOrdersByCustomerIdAsync(customerId);
            return Ok(orders);
        }

        // GET: api/Order/customer/{customerId}/status/{statusId}
        [HttpGet("customer/{customerId}/status/{statusId}")]
        public async Task<IActionResult> GetOrdersByCustomerAndStatus(int customerId, int statusId)
        {
            var orders = await _orderService.GetOrdersByCustomerAndStatusAsync(customerId, statusId);
            return Ok(orders);
        }

        // PUT: api/Order/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderUpdateRequestDTO request)
        {
            // Check if order exists before updating
            var existingOrder = await _orderService.GetOrderByIdAsync(id);
            if (existingOrder == null)
                return NotFound("Order not found");

            var updatedOrder = await _orderService.UpdateOrderAsync(id, request);
            if (updatedOrder == null)
                return BadRequest("Failed to update order");

            return Ok(updatedOrder);
        }

        // PUT: api/Order/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] int orderStatusId)
        {
            var updatedOrder = await _orderService.UpdateOrderStatusAsync(id, orderStatusId);
            if (updatedOrder == null)
                return NotFound();
            return Ok(updatedOrder);
        }

        // PUT: api/Order/{id}/cancel
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var result = await _orderService.CancelOrderAsync(id);
            if (!result)
                return BadRequest("Cannot cancel this order. Order may be already delivered or cancelled.");
            return Ok();
        }

        // GET: api/Order/history
        [HttpGet("history")]
        public async Task<ActionResult<List<OrderDTO>>> GetOrderHistory()
        {
            var orders = await _orderService.GetOrderHistoryAsync();
            return Ok(orders);
        }
    }
}
