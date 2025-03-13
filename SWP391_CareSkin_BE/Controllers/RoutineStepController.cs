using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOS.Requests.Routine;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.DTOS.Responses.Routine;
using SWP391_CareSkin_BE.Exceptions;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/routine-steps")]
    [ApiController]
    public class RoutineStepController : ControllerBase
    {
        private readonly IRoutineStepService _routineStepService;

        public RoutineStepController(IRoutineStepService routineStepService)
        {
            _routineStepService = routineStepService;
        }

        [HttpGet]
        public async Task<ActionResult<List<RoutineStepDTO>>> GetAllSteps()
        {
            try
            {
                var steps = await _routineStepService.GetAllRoutineStepsAsync();
                return Ok(steps);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoutineStepDTO>> GetStepById(int id)
        {
            try
            {
                var step = await _routineStepService.GetRoutineStepByIdAsync(id);
                return Ok(step);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("routine/{routineId}")]
        public async Task<ActionResult<List<RoutineStepDTO>>> GetStepsByRoutineId(int routineId)
        {
            try
            {
                var steps = await _routineStepService.GetRoutineStepsByRoutineIdAsync(routineId);
                return Ok(steps);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<RoutineStepDTO>> CreateStep([FromBody] RoutineStepCreateRequestDTO request)
        {
            try
            {
                var step = await _routineStepService.CreateRoutineStepAsync(request);
                return CreatedAtAction(nameof(GetStepById), new { id = step.RoutineStepId }, step);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RoutineStepDTO>> UpdateStep(int id, [FromBody] RoutineStepUpdateRequestDTO request)
        {
            try
            {
                var step = await _routineStepService.UpdateRoutineStepAsync(id, request);
                return Ok(step);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStep(int id)
        {
            try
            {
                await _routineStepService.DeleteRoutineStepAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
