using SWP391_CareSkin_BE.DTOS.Requests.Momo;
using SWP391_CareSkin_BE.DTOS.Responses.Momo;
using SWP391_CareSkin_BE.Models;
using System.Collections.Generic;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IMomoRepository
    {
        /// <summary>
        /// Creates a payment request to Momo and returns the payment URL
        /// </summary>
        Task<MomoPaymentResponseDto> CreatePaymentAsync(MomoPayment payment);

        /// <summary>
        /// Verifies the signature of the callback from Momo
        /// </summary>
        bool VerifySignature(MomoCallbackDto callbackDto);

        /// <summary>
        /// Saves a Momo payment to the database
        /// </summary>
        Task SaveMomoPaymentAsync(MomoPayment payment);

        /// <summary>
        /// Saves a Momo callback to the database
        /// </summary>
        Task SaveMomoCallbackAsync(MomoCallback callback);

        /// <summary>
        /// Gets a Momo payment by order ID
        /// </summary>
        Task<MomoPayment?> GetMomoPaymentByOrderIdAsync(int orderId);

        /// <summary>
        /// Updates a Momo payment's status
        /// </summary>
        Task UpdateMomoPaymentStatusAsync(string momoPaymentId, bool isPaid);

        /// <summary>
        /// Gets the latest callback for an order
        /// </summary>
        Task<MomoCallback?> GetLatestCallbackForOrderAsync(string orderId);

        /// <summary>
        /// Gets expired payments
        /// </summary>
        Task<IEnumerable<MomoPayment>> GetExpiredPaymentsAsync();

        /// <summary>
        /// Updates a Momo payment
        /// </summary>
        Task UpdatePaymentAsync(MomoPayment payment);

        /// <summary>
        /// Gets a Momo payment by Momo's orderId
        /// </summary>
        Task<MomoPayment?> GetMomoPaymentByMomoOrderIdAsync(string momoOrderId);

        /// <summary>
        /// Gets callbacks for an order
        /// </summary>
        Task<IEnumerable<MomoCallback>> GetCallbacksForOrderAsync(int orderId);
    }
}
