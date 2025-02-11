using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Models;



namespace  SWP391_CareSkin_BE.Data
.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MyDbContext _context;

        public AuthController(MyDbContext context)
        {
            _context = context;
        }

        [HttpPost ("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            //tìm user trong database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
            
            //so sáng user  
            if (user == null)
            {
                return Unauthorized(new {message ="Đăng nhập thất bại. Sai tên đăng nhập hoặc mật khẩu!!"});
            }

            //so sánh password 
            if(user.Password != request.Password)
            {
                return Unauthorized(new { message = "Đăng nhập thất  bại. Sai tên đăng nhập hoặc mặt khẩu!!" });
            }

            // đăng nhâjp thành công 
            return Ok("Đăng nhập thành công!");
        }
    }
}
