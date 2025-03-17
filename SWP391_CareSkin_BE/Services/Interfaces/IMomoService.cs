using SWP391_CareSkin_BE.DTOS.Requests.Momo;
using SWP391_CareSkin_BE.DTOS.Responses.Momo;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IMomoService
    {
        /// <summary>
        /// Creates a Momo payment for an order
        /// </summary>
        Task<MomoPaymentResponseDto> CreateMomoPaymentAsync(MomoPaymentRequestDto paymentRequestDto);

        /// <summary>
        /// Validates a callback from Momo
        /// </summary>
        bool ValidateMomoCallback(MomoCallbackDto callbackDto);

        /// <summary>
        /// Handles a callback from Momo
        /// </summary>
        Task HandleMomoCallbackAsync(MomoCallbackDto callbackDto);

        /// <summary>
        /// Checks the payment status of an order
        /// </summary>
        Task<MomoPaymentStatusDto> CheckPaymentStatusAsync(int orderId);
        
        /// <summary>
        /// Cancels expired payments
        /// </summary>
        Task CancelExpiredPayments();
        
        /// <summary>
        /// Creates a standardized error response
        /// </summary>
        MomoPaymentResponseDto CreateErrorResponse(string message, long amount);
    }
}
