using SWP391_CareSkin_BE.DTOs.Requests.Vnpay;
using SWP391_CareSkin_BE.DTOs.Responses.Vnpay;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IVnpayService
    {
        Task<string> CreatePaymentUrl(VnpayRequestDTO model, HttpContext context);
        VnpayResponseDTO PaymentExecute(IQueryCollection collection);
    }
}
