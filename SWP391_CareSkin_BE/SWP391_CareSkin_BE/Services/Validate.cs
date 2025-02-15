namespace SWP391_CareSkin_BE.Services
{
    public class Validate
    {
        public static bool VerifyPassword(string passwordCheck, string password)
        {
            return password == passwordCheck;
        }
    }
}
