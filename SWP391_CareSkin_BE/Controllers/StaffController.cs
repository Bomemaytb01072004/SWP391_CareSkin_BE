using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOS;
using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Services;
using SWP391_CareSkin_BE.Services.Implementations;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;
        private readonly IFirebaseService _firebaseService;

        public StaffController(IStaffService staffService, IFirebaseService firebaseService)
        {
            _staffService = staffService;
            _firebaseService = firebaseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStaff()
        {
            var staff = await _staffService.GetAllStaffAsync();
            return Ok(staff);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterStaffDTO request)
        {
            try
            {
                var staff = await _staffService.RegisterStaffAsync(request);
                return Ok(new { message = "Register account successful!", staff });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{staffId}")]
        public async Task<IActionResult> GetStaffById(int staffId)
        {
            var staff = await _staffService.GetStaffByIdAsync(staffId);
            if (staff == null) return NotFound(new { message = "Staff does not exist!" });
            return Ok(staff);
        }

        [HttpPut("{staffId}")]
        public async Task<IActionResult> UpdateProfile(int staffId, [FromForm] UpdateProfileStaffDTO request)
        {
            // 1. Nếu có file mới, upload file và lấy URL
            string newPictureUrl = null;
            if (request.PictureFile != null && request.PictureFile.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{request.PictureFile.FileName}";
                using var stream = request.PictureFile.OpenReadStream();

                newPictureUrl = await _firebaseService.UploadImageAsync(stream, fileName);
            }

            try
            {
                var updatedStaff = await _staffService.UpdateProfileAsync(staffId, request, newPictureUrl);
                return Ok(new { message = "Update account succesful!", updatedStaff });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{staffId}")]
        public async Task<IActionResult> DeleteAccount(int staffId, [FromBody] string password)
        {
            try
            {
                await _staffService.DeleteStaffAsync(staffId, password);
                return Ok(new { message = "Delete account successful!" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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

            var authResult = await _staffService.Login(loginDto);

            if (!authResult.Success)
            {
                return BadRequest(authResult.Message); 
            }

            return Ok(authResult);
        }
    }
}
