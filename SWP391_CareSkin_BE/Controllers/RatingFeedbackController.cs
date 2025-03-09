using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOS.RatingFeedback;
using SWP391_CareSkin_BE.Services.Interfaces;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingFeedbackController : ControllerBase
    {
        private readonly IRatingFeedbackService _ratingFeedbackService;
        private readonly ICustomerService _customerService;

        public RatingFeedbackController(IRatingFeedbackService ratingFeedbackService, ICustomerService customerService)
        {
            _ratingFeedbackService = ratingFeedbackService;
            _customerService = customerService;
        }

        private int GetCustomerIdFromClaims()
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "CustomerId");
            if (idClaim == null)
                return 0;

            if (int.TryParse(idClaim.Value, out int customerId))
                return customerId;

            return 0;
        }

        // GET: api/RatingFeedback/product/{productId}
        [HttpGet("RatingFeedback/product/{productId}")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            var ratingFeedbacks = await _ratingFeedbackService.GetRatingFeedbacksByProductIdAsync(productId);
            return Ok(ratingFeedbacks);
        }

        // GET: api/RatingFeedback/average/{productId}
        [HttpGet("RatingFeedback/average/{productId}")]
        public async Task<IActionResult> GetAverageRating(int productId)
        {
            var averageRating = await _ratingFeedbackService.GetAverageRatingForProductAsync(productId);
            return Ok(averageRating);
        }

        // GET: api/RatingFeedback/my-ratings
        [HttpGet("RatingFeedback/my-ratings")]
        public async Task<IActionResult> GetMyRatings()
        {
            var customerId = GetCustomerIdFromClaims();
            var customer = await _customerService.GetCustomerByIdAsync(customerId);
            if (customer == null)
                return NotFound("Customer not found");
                
            var ratingFeedbacks = await _ratingFeedbackService.GetRatingFeedbacksByCustomerIdAsync(customerId);
            return Ok(ratingFeedbacks);
        }

        // GET: api/RatingFeedback/{id}
        [HttpGet("RatingFeedback/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ratingFeedback = await _ratingFeedbackService.GetRatingFeedbackByIdAsync(id);
            if (ratingFeedback == null)
                return NotFound();

            return Ok(ratingFeedback);
        }

        // POST: api/RatingFeedback
        [HttpPost("RatingFeedback/{id}")]
        public async Task<IActionResult> Create(int id, [FromForm] CreateRatingFeedbackDTO createDto)
        {
            // Get customerId from DTO if provided (for Swagger testing), otherwise from claims
            //int customerId = createDto.CustomerId.HasValue ? createDto.CustomerId.Value : GetCustomerIdFromClaims();
            int customerId = id;
            var customer = await _customerService.GetCustomerByIdAsync(customerId);
            if (customer == null)
                return NotFound("Customer not found");
                
            var ratingFeedback = await _ratingFeedbackService.CreateRatingFeedbackAsync(customerId, createDto);
            return CreatedAtAction(nameof(GetById), new { id = ratingFeedback.RatingFeedbackId }, ratingFeedback);
        }

        // PUT: api/RatingFeedback/{id}
        [HttpPut("RatingFeedback/{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateRatingFeedbackDTO updateDto)
        {
            // Get customerId from DTO if provided (for Swagger testing), otherwise from claims
            //int customerId = updateDto.CustomerId.HasValue ? updateDto.CustomerId.Value : GetCustomerIdFromClaims();
            
            //var customer = await _customerService.GetCustomerByIdAsync(customerId);
            //if (customer == null)
            //    return NotFound("Customer not found");

            int customerId = updateDto.CustomerId;

            var ratingFeedback = await _ratingFeedbackService.UpdateRatingFeedbackAsync(customerId, id, updateDto);
            if (ratingFeedback == null)
                return NotFound();

            return Ok(ratingFeedback);
        }

        // DELETE: api/RatingFeedback/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            //var customerId = GetCustomerIdFromClaims();
            //var customer = await _customerService.GetCustomerByIdAsync(customerId);
            //if (customer == null)
            //    return NotFound("Customer not found");
                
            var result = await _ratingFeedbackService.DeleteRatingFeedbackAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
