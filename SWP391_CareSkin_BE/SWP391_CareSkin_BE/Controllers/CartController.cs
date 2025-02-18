using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET: api/Cart/customer/{customerId}
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCartItemsByCustomer(int customerId)
        {
            var cartItems = await _cartService.GetCartItemsByCustomerIdAsync(customerId);
            return Ok(cartItems);
        }

        // POST: api/Cart/add
        [HttpPost("add")]
        public async Task<IActionResult> AddCartItem([FromBody] CartCreateRequestDTO request)
        {
            var cartItem = await _cartService.AddCartItemAsync(request);
            return Ok(cartItem);
        }

        // PUT: api/Cart/update/{id}
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCartItem(int id, [FromBody] CartUpdateRequestDTO request)
        {
            // Đảm bảo request.Id = id
            request.CartId = id;
            var updatedCartItem = await _cartService.UpdateCartItemAsync(request);
            if (updatedCartItem == null)
                return NotFound();
            return Ok(updatedCartItem);
        }

        // DELETE: api/Cart/remove/{id}
        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> RemoveCartItem(int id)
        {
            var result = await _cartService.RemoveCartItemAsync(id);
            if (!result)
                return NotFound();
            return Ok(new { message = "Cart item removed successfully" });
        }
    }
}
