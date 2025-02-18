
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using Microsoft.AspNetCore.Identity.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.DTOS.Responses;



namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly MyDbContext _context;

        public RegisterController(MyDbContext context)
        {
            _context = context;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.Email)|| 
                request.Dob == default(DateTime) || 
                string.IsNullOrWhiteSpace(request.ProfilePicture) ||
                string.IsNullOrWhiteSpace(request.Gender) ||
                string.IsNullOrWhiteSpace(request.Address) ||
                string.IsNullOrWhiteSpace(request.FullName))


            {
                return BadRequest(new { message = "Tên đăng nhập, mật khẩu và email không được để trống!" });
            }

            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest(new { message = "Mật khẩu xác nhận không khớp!" });
            }

            var existingUser = await _context.Customers
                .FirstOrDefaultAsync(u => u.UserName == request.UserName || u.Email == request.Email);

            if (existingUser != null)
            {
                return Conflict(new { message = "Tên đăng nhập hoặc email đã tồn tại!" });
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var newUser = new Customer
            {
                UserName = request.UserName,
                Password = hashedPassword,
                Email = request.Email,
                Dob = request.Dob,
                ProfilePicture = request.ProfilePicture,
                Gender = request.Gender,
                Address = request.Address,
                FullName = request.FullName
            };

            await _context.Customers.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công!", userId = newUser.CustomerId });
        }
    }
}
