using Microsoft.AspNetCore.Http.HttpResults;
using SWP391_CareSkin_BE.DTOs.Requests.Vnpay;
using SWP391_CareSkin_BE.DTOs.Responses.Vnpay;
using SWP391_CareSkin_BE.Lib;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class VnpayService : IVnpayService
    {
        private readonly IConfiguration _configuration;
        private readonly IVnpayRepository _vnpayRepository;
        private readonly IOrderService _orderService;

        public VnpayService(IConfiguration configuration, IVnpayRepository vnpayRepository, IOrderService orderService)
        {
            _configuration = configuration;
            _vnpayRepository = vnpayRepository;
            _orderService = orderService;
        }

        public async Task<string> CreatePaymentUrl(VnpayRequestDTO model, HttpContext context)
        {
            //if (await _orderService.GetOrderByIdAsync(model.OrderId) != null)
            //{
            //    return "Đơn hàng không hợp lệ";
            //}

            //if (await _vnpayRepository.GetByOrderIdAsync(model.OrderId) != null)
            //{
            //    return "Đơn hàng đã được xử lý";
            //}

           
            long paymentAmount = ((int)model.Amount * 100);
            VnpayLibrary pay = new VnpayLibrary();

            pay.AddRequestData("vnp_Version", "2.1.0");
            pay.AddRequestData("vnp_Command", "pay");
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", (paymentAmount.ToString()));
            pay.AddRequestData("vnp_BankCode", "VNBANK");
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", "vn");
            pay.AddRequestData("vnp_TxnRef", model.OrderId + "_" + DateTime.Now.Ticks.ToString());
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang");
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", _configuration["Vnpay:PaymentBackReturnUrl"]);

            string paymentUrl = pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }
        public VnpayResponseDTO PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnpayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);

            var payment = new VnpayTransactions
            {
                TransactionId = int.Parse(response.TransactionId),//trans cua vnpay || dang lay id trans vnpay lam id trans 
                OrderId = response.OrderId,
                Amount = (decimal)response.Amount,
                CreatedAt = DateTime.Now,
                OrderDescription = response.OrderDescription,
                PaymentMethod = response.PaymentMethod,
                PaymentStatus = response.VnPayResponseCode == "00" ? "Success" : "Failed",
            };
            //2:success, 3:fail
            int statusId = response.VnPayResponseCode == "00" ? 2 : 3;//nen de status vao bang order luon cx dc

            _vnpayRepository.AddTransactionAsync(payment);//add vao bang transtion
            _orderService.UpdateOrderStatusAsync(response.OrderId, statusId);//update bang order

            return new VnpayResponseDTO
            {
                Success = response.VnPayResponseCode == "00",
                OrderDescription = response.OrderDescription,
                OrderId = response.OrderId,
                TransactionId = response.TransactionId,
                PaymentMethod = response.PaymentMethod,
                PaymentId = int.Parse(response.TransactionId).ToString(),//ko bt lay paymentid tu dau || dang lay tu trans id
                Token = response.Token,
                VnPayResponseCode = response.VnPayResponseCode,
                Amount = response.Amount,
            };//hoac retrun payment tren cx dc
        }
    }
}
