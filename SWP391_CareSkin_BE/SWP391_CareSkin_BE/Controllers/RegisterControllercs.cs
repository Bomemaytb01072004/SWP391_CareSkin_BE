﻿
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Data.Data;
using SWP391_CareSkin_BE.Models;


namespace SWP391_CareSkin_BE.Controllers
{
    public class RegisterController : ControllerBase
    {
        private readonly MyDbContext _context;

        public RegisterController(MyDbContext context)
        {
            _context = context;
        }

    
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(new { message = "Tên đăng nhập, mật khẩu và email không được để trống!" });
            }

            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest(new { message = "Mật khẩu xác nhận không khớp!" });
            }

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == request.UserName || u.Email == request.Email);

            if (existingUser != null)
            {
                return Conflict(new { message = "Tên đăng nhập hoặc email đã tồn tại!" });
            }

            var newUser = new User
            {
                UserName = request.UserName,
                Password = request.Password,
                Email = request.Email 
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công!", userId = newUser.IdUser });
        }
    }
}
