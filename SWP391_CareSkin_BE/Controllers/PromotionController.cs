using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Requests.Promotion;
using SWP391_CareSkin_BE.DTOS.Responses.Promotion;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        // GET: api/Promotion
        [HttpGet]
        public async Task<ActionResult<List<PromotionDTO>>> GetAllPromotions()
        {
            var promotions = await _promotionService.GetAllPromotionsAsync();
            return Ok(promotions);
        }

        // GET: api/Promotion/active
        [HttpGet("active")]
        public async Task<ActionResult<List<PromotionDTO>>> GetActivePromotions()
        {
            var promotions = await _promotionService.GetActivePromotionsAsync();
            return Ok(promotions);
        }

        // GET: api/Promotion/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PromotionDTO>> GetPromotionById(int id)
        {
            var promotion = await _promotionService.GetPromotionByIdAsync(id);
            if (promotion == null)
                return NotFound();
            return Ok(promotion);
        }

        // POST: api/Promotion/set-product-discount
        [HttpPost("set-product-discount")]
        public async Task<ActionResult<PromotionDTO>> SetProductDiscount([FromBody] SetProductDiscountRequestDTO request)
        {
            try
            {
                var result = await _promotionService.SetProductDiscountAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Promotion/product-discounts
        [HttpGet("product-discounts")]
        public async Task<ActionResult<List<ProductDiscountDTO>>> GetProductDiscounts()
        {
            try
            {
                var discounts = await _promotionService.GetProductDiscountsAsync();
                return Ok(discounts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Promotion/product-discount-status
        [HttpPut("product-discount-status")]
        public async Task<ActionResult> UpdateProductDiscountStatus([FromBody] UpdateProductDiscountStatusDTO request)
        {
            try
            {
                var result = await _promotionService.UpdateProductDiscountStatusAsync(request);
                return Ok(new { success = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Promotion
        [HttpPost]
        public async Task<ActionResult<PromotionDTO>> CreatePromotion([FromBody] PromotionCreateRequestDTO request)
        {
            try
            {
                var promotion = await _promotionService.CreatePromotionAsync(request);
                return CreatedAtAction(nameof(GetPromotionById), new { id = promotion.PromotionId }, promotion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the promotion: {ex.Message}");
            }
        }

        // PUT: api/Promotion/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<PromotionDTO>> UpdatePromotion(int id, [FromBody] PromotionUpdateRequestDTO request)
        {
            try
            {
                var promotion = await _promotionService.UpdatePromotionAsync(id, request);
                if (promotion == null)
                    return NotFound();
                return Ok(promotion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the promotion: {ex.Message}");
            }
        }

        // DELETE: api/Promotion/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePromotion(int id)
        {
            var result = await _promotionService.DeletePromotionAsync(id);
            if (!result)
                return NotFound();
            return Ok(new { message = "Promotion deleted successfully" });
        }

        // PUT: api/Promotion/{id}/deactivate
        [HttpPut("{id}/deactivate")]
        public async Task<ActionResult<PromotionDTO>> DeactivatePromotion(int id)
        {
            try
            {
                var promotion = await _promotionService.DeactivatePromotionAsync(id);
                if (promotion == null)
                    return NotFound();
                return Ok(promotion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deactivating the promotion: {ex.Message}");
            }
        }
    }
}
