using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOS.Requests.Routine;
using SWP391_CareSkin_BE.DTOS.Responses.Routine;
using SWP391_CareSkin_BE.Exceptions;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutineProductController : ControllerBase
    {
        private readonly IRoutineProductService _routineProductService;

        public RoutineProductController(IRoutineProductService routineProductService)
        {
            _routineProductService = routineProductService;
        }

        [HttpGet]
        public async Task<ActionResult<List<RoutineProductDTO>>> GetAllRoutineProducts()
        {
            var routineProducts = await _routineProductService.GetAllRoutineProductsAsync();
            return Ok(routineProducts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoutineProductDTO>> GetRoutineProductById(int id)
        {
            try
            {
                var routineProduct = await _routineProductService.GetRoutineProductByIdAsync(id);
                return Ok(routineProduct);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("routine/{routineId}")]
        public async Task<ActionResult<List<RoutineProductDTO>>> GetRoutineProductsByRoutineId(int routineId)
        {
            try
            {
                var routineProducts = await _routineProductService.GetRoutineProductsByRoutineIdAsync(routineId);
                return Ok(routineProducts);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<RoutineProductDTO>> CreateRoutineProduct([FromBody] RoutineProductCreateRequestDTO request)
        {
            try
            {
                var routineProduct = await _routineProductService.CreateRoutineProductAsync(request);
                return CreatedAtAction(nameof(GetRoutineProductById), new { id = routineProduct.RoutineProductId }, routineProduct);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<RoutineProductDTO>> UpdateRoutineProduct(int id, [FromBody] RoutineProductUpdateRequestDTO request)
        {
            try
            {
                var routineProduct = await _routineProductService.UpdateRoutineProductAsync(id, request);
                return Ok(routineProduct);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult> DeleteRoutineProduct(int id)
        {
            try
            {
                await _routineProductService.DeleteRoutineProductAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
