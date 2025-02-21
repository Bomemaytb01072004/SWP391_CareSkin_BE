using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Services;

namespace SWP391_CareSkin_BE.Controllers.StaffController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly MyDbContext _context;

        public StaffController(MyDbContext context)
        {
            _context = context;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterStaffDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.Email))

            {
                return BadRequest(new { message = "Username, password and email cannot be empty!" });
            }

            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest(new { message = "Password is incorrect!" });
            }

            var existingStaff = await _context.Customers
                .FirstOrDefaultAsync(u => u.UserName == request.UserName || u.Email == request.Email);

            if (existingStaff != null)
            {
                return Conflict(new { message = "UserName or Email already exists!" });
            }
            string hashedPassword = Validate.HashPassword(request.Password);
            var newStaff = StaffMapper.ToStaff(request, hashedPassword);
            await _context.Staffs.AddAsync(newStaff);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Register successfully!", userId = newStaff.StaffId});

        }

        [HttpPut("Update-Profile/{staffId}")]
        public async Task<IActionResult> UpdateProfile(int staffId, [FromBody] UpdateProfileStaffDTO updatedStaff)
        {
            var staff = await _context.Staffs.FindAsync(staffId);
            if (staff == null)
            {
                return NotFound(new { message = "UserName does not exsit!!" });
            }
            StaffMapper.UpdateStaff(staff,updatedStaff);
            _context.Staffs.Update(staff);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Update successfully!" });
        }

        [HttpDelete("Delete-Account/{staffId}")]
        public async Task<IActionResult> DeleteAccount(int staffId, [FromBody] DeleteAccountCustomerDTO request)
        {
            // tìm staff theo ID
            var staff = await _context.Staffs.FindAsync(staffId);
            if (staff == null)
            {
                return NotFound(new { message = "UserName does not exsit!" });
            }
            // kiểm tra mật khẩu nhập vào với mật khẩu hash trong database
            if (!Validate.VerifyPassword(staff.Password, request.Password))
            {
                return BadRequest(new { message = "Password is incorrect!" });
            }
            // Xóa tài khoản
            _context.Staffs.Remove(staff);
            await _context.SaveChangesAsync();

            return Ok(new { message = "The account has been successfully deleted!" });
        }

        [HttpGet("GetById/{staffId}")]
        public async Task<IActionResult> GetStaffById(int staffId)
        {
            var staff = await _context.Staffs.FindAsync(staffId);

            if (staff == null)
            {
                return NotFound(new { message = "User does not exsit!" });
            }
            var staffResponse = StaffMapper.ToStaffResponseDTO(staff);
            return Ok(staffResponse);
        }
    }
}
