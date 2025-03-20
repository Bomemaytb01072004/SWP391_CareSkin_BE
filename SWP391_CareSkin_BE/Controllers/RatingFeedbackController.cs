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
        [HttpGet("RatingFeedback/my-ratings/{customerId}")]
        public async Task<IActionResult> GetMyRatings(int customerId)
        {
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

        // GET: api/RatingFeedback
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RatingFeedbackDTO>>> GetAllRatingFeedbacks()
        {
            var ratingFeedbacks = await _ratingFeedbackService.GetAllRatingFeedbacksAsync();
            return Ok(ratingFeedbacks);
        }

        // GET: api/RatingFeedback/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<RatingFeedbackDTO>>> GetActiveRatingFeedbacks()
        {
            var ratingFeedbacks = await _ratingFeedbackService.GetActiveRatingFeedbacksAsync();
            return Ok(ratingFeedbacks);
        }

        // GET: api/RatingFeedback/inactive
        [HttpGet("inactive")]
        public async Task<ActionResult<IEnumerable<RatingFeedbackDTO>>> GetInactiveRatingFeedbacks()
        {
            var ratingFeedbacks = await _ratingFeedbackService.GetInactiveRatingFeedbacksAsync();
            return Ok(ratingFeedbacks);
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
