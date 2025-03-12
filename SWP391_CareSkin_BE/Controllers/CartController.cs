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
            if (cartItem == null) return BadRequest();
            return Ok(cartItem);
        }

        // PUT: api/Cart/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCartItem([FromBody] CartUpdateRequestDTO request)
        {
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
            return Ok();
        }

        // GET: api/Cart/total/{customerId}
        [HttpGet("total/{customerId}")]
        public async Task<IActionResult> GetCartTotal(int customerId)
        {
            var total = await _cartService.CalculateCartTotalPrice(customerId);
            return Ok(total);
        }
        
        // GET: api/Cart/total-sale/{customerId}
        [HttpGet("total-sale/{customerId}")]
        public async Task<IActionResult> GetCartTotalSalePrice(int customerId)
        {
            var totalSalePrice = await _cartService.CalculateCartTotalSalePrice(customerId);
            return Ok(totalSalePrice);
        }
        
        // GET: api/Cart/totals/{customerId}
        [HttpGet("totals/{customerId}")]
        public async Task<IActionResult> GetCartTotals(int customerId)
        {
            var (totalPrice, totalSalePrice) = await _cartService.CalculateCartTotals(customerId);
            return Ok(new { TotalPrice = totalPrice, TotalSalePrice = totalSalePrice });
        }
    }
}
