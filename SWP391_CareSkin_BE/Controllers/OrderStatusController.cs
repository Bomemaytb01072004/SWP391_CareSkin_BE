using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOS.Requests.OrderStatus;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatusController : ControllerBase
    {
        private readonly IOrderStatusService _orderStatusService;

        public OrderStatusController(IOrderStatusService orderStatusService)
        {
            _orderStatusService = orderStatusService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderStatuses()
        {
            var orderStatuses = await _orderStatusService.GetAllOrderStatusesAsync();
            return Ok(orderStatuses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderStatusById(int id)
        {
            var orderStatus = await _orderStatusService.GetOrderStatusByIdAsync(id);
            return Ok(orderStatus);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderStatus([FromBody] OrderStatusCreateRequestDTO dto)
        {
            var createdStatus = await _orderStatusService.CreateOrderStatusAsync(dto);
            return CreatedAtAction(nameof(GetOrderStatusById), new { id = createdStatus.OrderStatusId }, createdStatus);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatusUpdateRequestDTO dto)
        {
            var updatedStatus = await _orderStatusService.UpdateOrderStatusAsync(id, dto);
            return Ok(updatedStatus);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderStatus(int id)
        {
            await _orderStatusService.DeleteOrderStatusAsync(id);
            return NoContent();
        }
    }
}
