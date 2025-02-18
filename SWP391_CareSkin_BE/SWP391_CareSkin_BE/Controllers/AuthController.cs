using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOS;
using SWP391_CareSkin_BE.Helpers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Services;



namespace  SWP391_CareSkin_BE.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly JwtHelper _jwtHelper;


        public AuthController(MyDbContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            var admin = _context.Admins.FirstOrDefault(a => a.UserName == request.UserName);
            var customer = _context.Customers.FirstOrDefault(c => c.UserName == request.UserName);
            var staff = _context.Staffs.FirstOrDefault(s => s.UserName == request.UserName);

            object account = (object)admin ?? (object)staff ?? (object)customer;

            // check username
            if (account == null)
            {
                return Unauthorized("Sai tên đăng nhập hoặc thất bại!!!");
            }

            // check password
            string password = admin != null ? admin.Password : staff != null ? staff.Password : customer != null ? customer.Password : null;

            if (!Validate.VerifyPassword(password, request.Password))
            {
                return Unauthorized("Sai tên đăng nhập hoặc mật khẩu!!!!");
            };

            // xác định role
            string role = admin != null ? "Admin" : staff != null ? "Staff" : "Customer";

            // Tạo token JWT
            var token = _jwtHelper.GenerateToken(request.UserName, role);

            // trả về token
            return Ok(new { token });
        }


    }
}
