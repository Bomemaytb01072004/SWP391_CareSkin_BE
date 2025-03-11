using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOS.Requests.Result;
using SWP391_CareSkin_BE.DTOS.Responses.Result;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultController : ControllerBase
    {
        private readonly IResultService _resultService;

        public ResultController(IResultService resultService)
        {
            _resultService = resultService;
        }

        [HttpPost]
        public async Task<ActionResult<ResultDTO>> CreateResult([FromBody] CreateResultDTO createResultDTO)
        {
            try
            {
                var result = await _resultService.CreateResultAsync(createResultDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{resultId}")]
        public async Task<ActionResult<ResultDTO>> GetResultById(int resultId)
        {
            try
            {
                var result = await _resultService.GetResultByIdAsync(resultId);
                
                if (result == null)
                {
                    return NotFound($"Result with ID {resultId} not found");
                }
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<List<ResultDTO>>> GetResultsByCustomerId(int customerId)
        {
            try
            {
                var results = await _resultService.GetResultsByCustomerIdAsync(customerId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("quiz/{quizId}/customer/{customerId}")]
        public async Task<ActionResult<ResultDTO>> GetLatestResultByQuizAndCustomer(int quizId, int customerId)
        {
            try
            {
                var result = await _resultService.GetLatestResultByQuizAndCustomerAsync(quizId, customerId);
                
                if (result == null)
                {
                    return NotFound($"No result found for quiz ID {quizId} and customer ID {customerId}");
                }
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
