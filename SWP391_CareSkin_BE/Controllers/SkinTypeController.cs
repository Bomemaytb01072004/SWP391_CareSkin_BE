using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkinTypeController : ControllerBase
    {
        private readonly ISkinTypeService _skinTypeService;

        public SkinTypeController(ISkinTypeService skinTypeService)
        {
            _skinTypeService = skinTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SkinTypeDTO>>> GetAll()
        {
            var skinTypes = await _skinTypeService.GetAllAsync();
            return Ok(skinTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SkinTypeDTO>> GetById(int id)
        {
            var skinType = await _skinTypeService.GetByIdAsync(id);
            return Ok(skinType);
        }

        [HttpPost]
        public async Task<ActionResult<SkinTypeDTO>> Create([FromBody] SkinTypeCreateRequestDTO request)
        {
            var skinType = await _skinTypeService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = skinType.SkinTypeId }, skinType);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SkinTypeDTO>> Update(int id, [FromBody] SkinTypeUpdateRequestDTO request)
        {
            var skinType = await _skinTypeService.UpdateAsync(id, request);
            return Ok(skinType);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _skinTypeService.DeleteAsync(id);
            return NoContent();
        }
    }
}
