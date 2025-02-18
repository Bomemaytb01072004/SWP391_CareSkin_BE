namespace SWP391_CareSkin_BE.Services
{
    public class Validate
    {
        public static bool VerifyPassword(string hashedPassword, string plainPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }
    }
}
