using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOS;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Services;



namespace  SWP391_CareSkin_BE.Data.Controllers
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

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            var admin = _context.Admins.FirstOrDefault(a => a.UserName == request.UserName);
            var customer = _context.Customers.FirstOrDefault(c => c.UserName == request.UserName);
            var staff = _context.Staffs.FirstOrDefault(s => s.UserName == request.UserName);

            object account = (object)admin ?? (object)staff ?? (object)customer;

            //check username
            if (account == null)
            {
                return Unauthorized("Sai tên đăng nhập hoặc thất bại!!!");
            }


            //check password
            string password = admin != null ? admin.Password : staff != null ? staff.Password : customer != null ? customer.Password : null;

            if (!Validate.VerifyPassword(password, request.Password)) 
            {
                return Unauthorized("Sai tên đăng nhập hoặc mật khẩu!!!!");
            }

            //xac dinh role
            string role = admin != null ? "Admin" : staff != null ? "Staff" : "Customer";

            //luu role va username
            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.UserName),
                new Claim(ClaimTypes.Role, role)
            };

            // 
            var identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


            // đăng nhập thành công 
            return Ok(new { message = "Đăng nhập thành công!!!" });
        }

        
    }
}
