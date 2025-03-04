using SWP391_CareSkin_BE.DTOs.Requests.Vnpay;
using SWP391_CareSkin_BE.DTOs.Responses.Vnpay;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IVnpayService
    {
        Task<VnpayResponseDTO> CreatePaymentAsync(VnpayRequestDTO request);
        bool ValidateSignature(Dictionary<string, string> queryParams);
        Task<bool> ProcessPaymentCallbackAsync(Dictionary<string, string> queryParams);
    }
}
