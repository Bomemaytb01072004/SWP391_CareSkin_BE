namespace SWP391_CareSkin_BE.Services
{
    public class Validate
    {
        //mã hóa mật khẩu
        public static string HashPassword(string plainPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }


        //kiểm tra mật khẩu khi đăng nhập
        public static bool VerifyPassword(string hashedPassword, string plainPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }
    }
}
