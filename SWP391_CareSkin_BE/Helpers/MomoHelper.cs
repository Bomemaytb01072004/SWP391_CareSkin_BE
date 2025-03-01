using System.Security.Cryptography;
using System.Text;

namespace SWP391_CareSkin_BE.Helpers
{
    public static class MomoHelper
    {
        public static string CreateSignature(string rawData, string secretKey)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
