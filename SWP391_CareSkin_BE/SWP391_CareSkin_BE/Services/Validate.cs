namespace SWP391_CareSkin_BE.Services
{
    public class Validate
    {
        public static bool VerifyPassword(string password, string plainPassword)
        {
            return password == plainPassword;
        }
    }
}
