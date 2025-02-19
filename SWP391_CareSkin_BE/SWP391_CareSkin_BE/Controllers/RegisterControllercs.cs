
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
                string.IsNullOrWhiteSpace(request.Email))

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


            var newUser = new Customer
            {
                UserName = request.UserName,
                Password = request.Password,
                Email = request.Email,
                Dob = request.Dob , 
                ProfilePicture = request.ProfilePicture ?? "",
                Gender = request.Gender ?? "Unknown",
                Address = request.Address ?? "Not provided",
                FullName = request.FullName ?? "No name"
            };

            await _context.Customers.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công!", userId = newUser.CustomerId });
        }
    }
}
