using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email with the specified parameters
        /// </summary>
        /// <param name="toEmail">Recipient email address</param>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Email body content (HTML format)</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task SendEmailAsync(string toEmail, string subject, string body);
        
        /// <summary>
        /// Sends an order confirmation email to a customer
        /// </summary>
        /// <param name="toEmail">Customer email address</param>
        /// <param name="orderId">Order ID</param>
        /// <param name="customerName">Customer name</param>
        /// <param name="orderTotal">Order total amount</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task SendOrderConfirmationEmailAsync(string toEmail, string orderId, string customerName, decimal orderTotal);
        
        /// <summary>
        /// Sends a payment confirmation email to a customer
        /// </summary>
        /// <param name="toEmail">Customer email address</param>
        /// <param name="orderId">Order ID</param>
        /// <param name="customerName">Customer name</param>
        /// <param name="paymentAmount">Payment amount</param>
        /// <param name="paymentMethod">Payment method (e.g., Momo, VnPay)</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task SendPaymentConfirmationEmailAsync(string toEmail, string orderId, string customerName, decimal paymentAmount, string paymentMethod);
    }
}
