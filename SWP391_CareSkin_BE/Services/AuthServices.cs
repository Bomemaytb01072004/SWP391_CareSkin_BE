using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.DTOS;
using SWP391_CareSkin_BE.Helpers;
using SWP391_CareSkin_BE.Data;

namespace SWP391_CareSkin_BE.Services
{
    public class AuthServices
    {
        public class AuthService
        {
            private readonly MyDbContext _context;
            private readonly JwtHelper _jwtHelper;

            public AuthService(MyDbContext context, JwtHelper jwtHelper)
            {
                _context = context;
                _jwtHelper = jwtHelper;
            }

            public string Authenticate(string userName, string password)
            {
                // Kiểm tra Admin
                var admin = _context.Admins.FirstOrDefault(a => a.UserName == userName && a.Password == password);
                if (admin != null)
                    return _jwtHelper.GenerateToken(admin.UserName, "Admin");

                // Kiểm tra Staff
                var staff = _context.Staffs.FirstOrDefault(s => s.UserName == userName && s.Password == password);
                if (staff != null)
                    return _jwtHelper.GenerateToken(staff.UserName, "Staff");

                // Kiểm tra User
                var user = _context.Customers.FirstOrDefault(u => u.UserName == userName && u.Password == password);
                if (user != null)
                    return _jwtHelper.GenerateToken(user.UserName, "Customer");

                return null; // Tài khoản không tồn tại hoặc mật khẩu sai
            }
        }
    }
}
