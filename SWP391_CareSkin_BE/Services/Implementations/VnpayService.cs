using SWP391_CareSkin_BE.DTOs.Requests.Vnpay;
using SWP391_CareSkin_BE.DTOs.Responses.Vnpay;
using SWP391_CareSkin_BE.Lib;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class VnpayService : IVnpayService
    {
        private readonly IConfiguration _configuration;
        private readonly IVnpayRepository _vnpayRepository;

        public VnpayService(IConfiguration configuration, IVnpayRepository vnpayRepository)
        {
            _configuration = configuration;
            _vnpayRepository = vnpayRepository;
        }

        public string CreatePaymentUrl(VnpayRequestDTO model, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var transactionId = DateTime.Now.Ticks.ToString();
            var pay = new VnpayLibrary();
            var urlCallBack = _configuration["Vnpay:PaymentBackReturnUrl"];

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{model.CustomerName} {model.OrderDescription} {model.Amount}");
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", transactionId);

            var paymentUrl = pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }

        public VnpayResponseDTO PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnpayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);

            return new VnpayResponseDTO
            {
                Success = response.Success,
                OrderDescription = response.OrderDescription,
                OrderId = response.OrderId,
                TransactionId = response.TransactionId,
                PaymentMethod = response.PaymentMethod,
                PaymentId = response.PaymentId,
                Token = response.Token,
                VnPayResponseCode = response.VnPayResponseCode
            };
        }

    }
}
