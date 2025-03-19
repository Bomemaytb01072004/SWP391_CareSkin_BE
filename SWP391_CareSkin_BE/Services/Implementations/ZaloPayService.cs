using Newtonsoft.Json;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class ZaloPayService : IZaloPayService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public ZaloPayService(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<Dictionary<string, object>> CreateOrderAsync(ZaloPayOrder order)
        {
            var key1 = _config["ZaloPay:Key1"];
            var data = $"{order.AppId}|{order.AppTransId}|{order.AppUser}|{order.Amount}|{order.AppTime}|{order.EmbedData}|{order.Items}";
            order.Mac = ComputeHmac(data, key1);

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "app_id", order.AppId.ToString() },
                { "app_user", order.AppUser },
                { "app_trans_id", order.AppTransId },
                { "app_time", order.AppTime.ToString() },
                { "amount", order.Amount.ToString("F0") },
                { "description", order.Description },
                { "embed_data", order.EmbedData },
                { "item", order.Items },
                { "bank_code", order.BankCode },
                { "mac", order.Mac }
            });

            var response = await _httpClient.PostAsync(_config["ZaloPay:CreateOrderUrl"], content);
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(responseString);
        }

        public async Task<Dictionary<string, object>> QueryOrderAsync(string appTransId)
        {
            var appId = _config["ZaloPay:AppId"];
            var key1 = _config["ZaloPay:Key1"];
            var data = $"{appId}|{appTransId}|{key1}";
            var mac = ComputeHmac(data, key1);

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "app_id", appId },
                { "app_trans_id", appTransId },
                { "mac", mac }
            });

            var response = await _httpClient.PostAsync(_config["ZaloPay:QueryOrderUrl"], content);
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(responseString);
        }

        public string ComputeHmac(string data, string key)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
