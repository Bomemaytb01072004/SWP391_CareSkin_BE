using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SWP391_CareSkin_BE.DTOs.Requests.ZaloPay;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZaloPayController : ControllerBase
    {
        private readonly IZaloPayService _zaloPayService;
        private readonly IConfiguration _config;

        public ZaloPayController(IZaloPayService zaloPayService, IConfiguration config)
        {
            _zaloPayService = zaloPayService;
            _config = config;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] ZaloPayRequest request)
        {
            var appId = long.Parse(_config["ZaloPay:AppId"]);
            var appTransId = $"{DateTime.Now:yyMMdd}_{Guid.NewGuid():N}";

            // Tạo URL redirect với orderId đã được thêm vào
            var baseRedirectUrl = _config["ZaloPay:RedirectUrl"];
            var redirectUrl = $"{baseRedirectUrl}?orderId={request.OrderId}";

            // Tạo embed_data với redirect URL đã bao gồm orderId
            var embedData = new
            {
                redirecturl = redirectUrl, // URL redirect đã bao gồm orderId
                promotioninfo = ""
            };

            var order = new ZaloPayOrder
            {
                AppId = appId,
                AppUser = "user123",
                AppTransId = appTransId,
                AppTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Amount = request.Amount,
                Description = $"Thanh toán cho đơn hàng #{request.OrderId}",
                Items = "[]",
                EmbedData = JsonConvert.SerializeObject(embedData),
                BankCode = ""
            };

            var result = await _zaloPayService.CreateOrderAsync(order);
            return Ok(result);
        }

        [HttpPost("callback")]
        public IActionResult Callback([FromBody] dynamic callbackData)
        {
            var key2 = _config["ZaloPay:Key2"];
            var data = callbackData.data.ToString();
            var receivedMac = callbackData.mac.ToString();

            var computedMac = _zaloPayService.ComputeHmac(data, key2);
            if (receivedMac != computedMac) return BadRequest("Invalid MAC");

            // Xử lý dữ liệu thanh toán thành công
            var paymentData = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            return Ok(new { return_code = 1, return_message = "Success" });
        }

        [HttpGet("query")]
        public async Task<IActionResult> QueryOrder(string appTransId)
        {
            var result = await _zaloPayService.QueryOrderAsync(appTransId);
            return Ok(result);
        }

        // Giữ lại phương thức redirect để xử lý trường hợp ZaloPay gọi đến endpoint này
        [HttpGet("redirect")]
        public IActionResult RedirectHandler()
        {
            // Lấy tất cả các tham số từ query string
            var appTransId = Request.Query["apptransid"].ToString();
            var status = Request.Query["status"].ToString();
            var orderId = Request.Query["orderId"].ToString(); // Thử lấy orderId từ query string nếu có
            
            if (string.IsNullOrEmpty(appTransId))
            {
                return BadRequest("Missing apptransid parameter");
            }

            // Nếu không có orderId trong query string, thử lấy từ embed_data
            if (string.IsNullOrEmpty(orderId))
            {
                try
                {
                    var queryResult = _zaloPayService.QueryOrderAsync(appTransId).Result;
                    if (queryResult.ContainsKey("embed_data"))
                    {
                        var embedData = JsonConvert.DeserializeObject<Dictionary<string, object>>(queryResult["embed_data"].ToString());
                        if (embedData.ContainsKey("orderId"))
                        {
                            orderId = embedData["orderId"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log lỗi nhưng vẫn tiếp tục redirect
                    Console.WriteLine($"Error retrieving orderId: {ex.Message}");
                }
            }

            // Redirect về trang web với orderId và trạng thái thanh toán
            var baseUrl = _config["ZaloPay:RedirectUrl"];
            var paymentStatus = status == "1" ? "success" : "failed";
            
            // Thêm orderId vào URL redirect
            return Redirect($"{baseUrl}?status={paymentStatus}&orderId={orderId}&apptransid={appTransId}");
        }
    }
}
