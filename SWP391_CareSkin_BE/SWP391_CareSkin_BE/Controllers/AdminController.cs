using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOs.Requests.Admin;
using SWP391_CareSkin_BE.DTOs.Responses;
using SWP391_CareSkin_BE.DTOS;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Services;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // GET: api/Admin 

        [HttpGet]
        public async Task<IActionResult> GetAdmin()
        {
            var adminList = await _adminService.GetAdminAsync();
            return Ok(adminList);
        }

        // GET: api/Admin/{id}
        [HttpPut("{id}")]
        //[Authorize]
        public async Task<IActionResult> UpdateAdmin(int id, [FromForm] AdminUpdateRequestDTO request)
        {
            var updateAdmin = await _adminService.UpdateAdminAsync(request, id);
            if (updateAdmin == null)
            {
                return NotFound();
            }
            return Ok(updateAdmin);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO adminDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            LoginDTO loginDto = new LoginDTO
            {
                UserName = adminDTO.UserName,
                Password = adminDTO.Password
            };

            var authResult = await _adminService.Login(loginDto);

            if (!authResult.Success) 
            {
                return Unauthorized("Login failed");
            }

            return Ok( new LoginResult
            {
                Success = authResult.Success,
                Message = authResult.Message,
                Data = authResult.Data
            });
        }
    }
}
