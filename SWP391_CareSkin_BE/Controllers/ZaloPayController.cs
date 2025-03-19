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

            // Tạo embed_data với redirect URL
            var embedData = new
            {
                redirecturl = _config["ZaloPay:RedirectUrl"], // Lấy từ config
                promotioninfo = ""
            };

            var order = new ZaloPayOrder
            {
                AppId = appId,
                AppUser = "user123",
                AppTransId = appTransId,
                AppTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Amount = request.Amount,
                Description = $"Thanh toán cho đơn hàng #{appTransId}",
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

        [HttpGet("redirect")]
        public IActionResult RedirectHandler([FromQuery] ZaloPayRedirect model)
        {
            // 1. Validate checksum
            var key2 = _config["ZaloPay:Key2"];
            var data = $"{model.AppId}|{model.AppTransId}|{model.Pmcid}|{model.BankCode}|{model.Amount}|{model.DiscountAmount}|{model.Status}";
            var computedChecksum = _zaloPayService.ComputeHmac(data, key2);

            if (model.Checksum != computedChecksum)
                return BadRequest("Invalid checksum");

            // 2. Redirect với tham số trạng thái
            var baseUrl = _config["ZaloPay:RedirectUrl"];
            var status = model.Status == "1" ? "success" : "failed";
            return Redirect($"{baseUrl}?status={status}&orderId={model.AppTransId}");
        }
    }
}
