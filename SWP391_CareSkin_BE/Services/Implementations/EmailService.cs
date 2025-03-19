using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Sends an email with the specified parameters
        /// </summary>
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();
                
                // Set sender information
                email.From.Add(new MailboxAddress(_configuration["SmtpSettings:SenderName"], _configuration["SmtpSettings:SenderEmail"]));
                
                // Set recipient information
                email.To.Add(new MailboxAddress("", toEmail));
                
                // Set email subject
                email.Subject = subject;

                // Create HTML content
                email.Body = new TextPart("html") { Text = body };

                // Connect and send email via SMTP server
                using var smtp = new SmtpClient();
                
                // Connect to SMTP server (Using TLS)
                await smtp.ConnectAsync(
                    _configuration["SmtpSettings:Server"], 
                    int.Parse(_configuration["SmtpSettings:Port"]), 
                    SecureSocketOptions.StartTls);
                
                // Authenticate with email and password
                await smtp.AuthenticateAsync(
                    _configuration["SmtpSettings:SenderEmail"], 
                    _configuration["SmtpSettings:Password"]);
                
                // Send email
                await smtp.SendAsync(email);

                // Disconnect after sending
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception($"Failed to send email: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Sends an order confirmation email to a customer
        /// </summary>
        //public async Task SendOrderConfirmationEmailAsync(string toEmail, string orderId, string customerName, decimal orderTotal)
        //{
        //    string subject = $"CareSkin - Xác nhận đơn hàng #{orderId}";
            
        //    string body = $@"
        //    <html>
        //    <head>
        //        <style>
        //            body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        //            .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        //            .header {{ background-color: #f8bbd0; color: #fff; padding: 15px; text-align: center; }}
        //            .content {{ padding: 20px; border: 1px solid #ddd; }}
        //            .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #777; }}
        //            .button {{ display: inline-block; background-color: #f8bbd0; color: #fff; padding: 10px 20px; text-decoration: none; border-radius: 5px; }}
        //        </style>
        //    </head>
        //    <body>
        //        <div class='container'>
        //            <div class='header'>
        //                <h1>Xác nhận đơn hàng</h1>
        //            </div>
        //            <div class='content'>
        //                <p>Xin chào <strong>{customerName}</strong>,</p>
        //                <p>Cảm ơn bạn đã đặt hàng tại CareSkin Beauty Shop. Đơn hàng của bạn đã được xác nhận thành công.</p>
        //                <p><strong>Thông tin đơn hàng:</strong></p>
        //                <ul>
        //                    <li>Mã đơn hàng: <strong>#{orderId}</strong></li>
        //                    <li>Tổng thanh toán: <strong>${orderTotal:N0}</strong></li>
        //                </ul>
        //                <p>Chúng tôi sẽ tiến hành xử lý đơn hàng của bạn trong thời gian sớm nhất.</p>
        //                <p>Bạn có thể theo dõi trạng thái đơn hàng trong tài khoản của mình trên website của chúng tôi.</p>
        //                <p>Nếu bạn có bất kỳ câu hỏi nào, vui lòng liên hệ với chúng tôi qua email hoặc hotline.</p>
        //                <p>Trân trọng,</p>
        //                <p><strong>Đội ngũ CareSkin Beauty Shop</strong></p>
        //            </div>
        //            <div class='footer'>
        //                <p>© {DateTime.Now.Year} CareSkin Beauty Shop. All rights reserved.</p>
        //                <p>Email này được gửi tự động, vui lòng không trả lời.</p>
        //            </div>
        //        </div>
        //    </body>
        //    </html>";

        //    await SendEmailAsync(toEmail, subject, body);
        //}

        /// <summary>
        /// Sends a payment confirmation email to a customer
        /// </summary>
        //public async Task SendPaymentConfirmationEmailAsync(string toEmail, string orderId, string customerName, decimal paymentAmount, string paymentMethod)
        //{
        //    string subject = $"CareSkin - Xác nhận thanh toán đơn hàng #{orderId}";
            
        //    string body = $@"
        //    <html>
        //    <head>
        //        <style>
        //            body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        //            .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        //            .header {{ background-color: #f8bbd0; color: #fff; padding: 15px; text-align: center; }}
        //            .content {{ padding: 20px; border: 1px solid #ddd; }}
        //            .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #777; }}
        //            .button {{ display: inline-block; background-color: #f8bbd0; color: #fff; padding: 10px 20px; text-decoration: none; border-radius: 5px; }}
        //            .payment-info {{ background-color: #f9f9f9; padding: 15px; margin: 15px 0; border-left: 4px solid #f8bbd0; }}
        //        </style>
        //    </head>
        //    <body>
        //        <div class='container'>
        //            <div class='header'>
        //                <h1>Xác nhận thanh toán</h1>
        //            </div>
        //            <div class='content'>
        //                <p>Xin chào <strong>{customerName}</strong>,</p>
        //                <p>Chúng tôi xin thông báo rằng thanh toán cho đơn hàng của bạn đã được xác nhận thành công.</p>
                        
        //                <div class='payment-info'>
        //                    <p><strong>Thông tin thanh toán:</strong></p>
        //                    <ul>
        //                        <li>Mã đơn hàng: <strong>#{orderId}</strong></li>
        //                        <li>Số tiền: <strong>${paymentAmount:N0}</strong></li>
        //                        <li>Phương thức thanh toán: <strong>{paymentMethod}</strong></li>
        //                        <li>Thời gian: <strong>{DateTime.Now:dd/MM/yyyy HH:mm:ss}</strong></li>
        //                    </ul>
        //                </div>
                        
        //                <p>Đơn hàng của bạn đang được xử lý và sẽ được giao trong thời gian sớm nhất.</p>
        //                <p>Bạn có thể theo dõi trạng thái đơn hàng trong tài khoản của mình trên website của chúng tôi.</p>
        //                <p>Cảm ơn bạn đã mua sắm tại CareSkin Beauty Shop!</p>
        //                <p>Trân trọng,</p>
        //                <p><strong>Đội ngũ CareSkin Beauty Shop</strong></p>
        //            </div>
        //            <div class='footer'>
        //                <p>© {DateTime.Now.Year} CareSkin Beauty Shop. All rights reserved.</p>
        //                <p>Email này được gửi tự động, vui lòng không trả lời.</p>
        //            </div>
        //        </div>
        //    </body>
        //    </html>";

        //    await SendEmailAsync(toEmail, subject, body);
        //}

        public async Task SendOrderConfirmationEmailAsync(string toEmail, string orderId, string customerName, decimal orderTotal)
        {
            string subject = $"CareSkin - Xác nhận đơn hàng #{orderId}";

            string body = $@"
    <html>
    <head>
        <style>
            @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap');
            
            body {{ 
                font-family: 'Poppins', Arial, sans-serif; 
                line-height: 1.6; 
                color: #4b5563; 
                background-color: #f3f4f6; 
                margin: 0; 
                padding: 0; 
            }}
            
            .container {{ 
                max-width: 600px; 
                margin: 0 auto; 
                padding: 20px; 
            }}
            
            .email-wrapper {{ 
                background-color: #ffffff; 
                border-radius: 16px; 
                overflow: hidden; 
                box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08); 
            }}
            
            .header {{ 
                background: linear-gradient(135deg, #059669 0%, #047857 100%);
                color: #fff; 
                padding: 30px; 
                text-align: center; 
                position: relative; 
                overflow: hidden; 
            }}
            
            .header-pattern {{ 
                position: absolute; 
                inset: 0; 
                background-image: url('https://via.placeholder.com/600x200?text='); 
                opacity: 0.1; 
                background-size: cover;
            }}
            
            .header h1 {{ 
                position: relative; 
                z-index: 10; 
                margin: 0; 
                font-size: 28px; 
                font-weight: 600;
                letter-spacing: 0.5px;
            }}
            
            .content {{ 
                padding: 40px 30px; 
                background-color: #fff; 
            }}
            
            .greeting {{
                font-size: 18px;
                margin-bottom: 20px;
            }}
            
            .order-details {{ 
                background-color: #f0fdfa; 
                border-radius: 12px; 
                padding: 25px; 
                margin: 25px 0; 
                border-left: 4px solid #059669; 
                box-shadow: 0 2px 10px rgba(5, 150, 105, 0.06);
            }}
            
            .order-details h3 {{
                margin-top: 0; 
                color: #059669;
                font-size: 18px;
                font-weight: 600;
            }}
            
            .order-total {{ 
                font-size: 20px; 
                font-weight: 700; 
                color: #059669; 
                margin-top: 15px;
                padding-top: 15px;
                border-top: 2px solid #d1fae5;
                text-align: right;
            }}
            
            .action-button {{ 
                display: inline-block; 
                background: linear-gradient(to right, #059669, #047857); 
                color: #fff !important; 
                padding: 14px 32px; 
                text-decoration: none; 
                border-radius: 30px; 
                font-weight: 600; 
                margin-top: 20px; 
                box-shadow: 0 4px 12px rgba(5, 150, 105, 0.2);
                transition: all 0.3s ease;
                font-size: 16px;
                text-align: center;
            }}
            
            .action-button:hover {{ 
                transform: translateY(-2px);
                box-shadow: 0 6px 15px rgba(5, 150, 105, 0.25);
            }}
            
            .footer {{ 
                text-align: center; 
                padding: 25px 20px; 
                font-size: 13px; 
                color: #9ca3af; 
                background-color: #f9fafb;
                border-top: 1px solid #f3f4f6;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='email-wrapper'>
                <div class='header'>
                    <div class='header-pattern'></div>
                    <h1>Xác nhận đơn hàng</h1>
                </div>
                
                <div class='content'>
                    <p class='greeting'>Xin chào <strong>{customerName}</strong>,</p>
                    
                    <p>Cảm ơn bạn đã đặt hàng tại CareSkin Beauty Shop. Đơn hàng của bạn đã được xác nhận thành công.</p>
                    
                    <div class='order-details'>
                        <h3>Chi tiết đơn hàng</h3>
                        <p>Mã đơn hàng: <strong>#{orderId}</strong></p>
                        <p>Tổng thanh toán: <strong>${orderTotal:N0}</strong></p>
                    </div>
                    
                    <p>Chúng tôi sẽ tiến hành xử lý đơn hàng của bạn trong thời gian sớm nhất.</p>
                    
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='http://careskinbeauty.shop/' class='action-button'>Xem chi tiết đơn hàng</a>
                    </div>
                </div>
                
                <div class='footer'>
                    <p>© {DateTime.Now.Year} CareSkin Beauty Shop. All rights reserved.</p>
                    <p>Email này được gửi tự động, vui lòng không trả lời.</p>
                </div>
            </div>
        </div>
    </body>
    </html>";

            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendPaymentConfirmationEmailAsync(string toEmail, string orderId, string customerName, decimal paymentAmount, string paymentMethod)
        {
            string subject = $"CareSkin - Xác nhận thanh toán đơn hàng #{orderId}";

            string body = $@"
    <html>
    <head>
        <style>
            @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap');
            
            body {{ 
                font-family: 'Poppins', Arial, sans-serif; 
                line-height: 1.6; 
                color: #4b5563; 
                background-color: #f3f4f6; 
                margin: 0; 
                padding: 0; 
            }}
            
            .container {{ 
                max-width: 600px; 
                margin: 0 auto; 
                padding: 20px; 
            }}
            
            .email-wrapper {{ 
                background-color: #ffffff; 
                border-radius: 16px; 
                overflow: hidden; 
                box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08); 
            }}
            
            .header {{ 
                background: linear-gradient(135deg, #059669 0%, #047857 100%);
                color: #fff; 
                padding: 30px; 
                text-align: center; 
                position: relative; 
                overflow: hidden; 
            }}
            
            .header-pattern {{ 
                position: absolute; 
                inset: 0; 
                background-image: url('https://via.placeholder.com/600x200?text='); 
                opacity: 0.1; 
                background-size: cover;
            }}
            
            .header h1 {{ 
                position: relative; 
                z-index: 10; 
                margin: 0; 
                font-size: 28px; 
                font-weight: 600;
                letter-spacing: 0.5px;
            }}
            
            .content {{ 
                padding: 40px 30px; 
                background-color: #fff; 
            }}
            
            .greeting {{
                font-size: 18px;
                margin-bottom: 20px;
            }}
            
            .payment-details {{ 
                background-color: #f0fdfa; 
                border-radius: 12px; 
                padding: 25px; 
                margin: 25px 0; 
                border-left: 4px solid #059669; 
                box-shadow: 0 2px 10px rgba(5, 150, 105, 0.06);
            }}
            
            .payment-details h3 {{
                margin-top: 0; 
                color: #059669;
                font-size: 18px;
                font-weight: 600;
            }}
            
            .payment-total {{ 
                font-size: 20px; 
                font-weight: 700; 
                color: #059669; 
                margin-top: 15px;
                padding-top: 15px;
                border-top: 2px solid #d1fae5;
                text-align: right;
            }}
            
            .footer {{ 
                text-align: center; 
                padding: 25px 20px; 
                font-size: 13px; 
                color: #9ca3af; 
                background-color: #f9fafb;
                border-top: 1px solid #f3f4f6;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='email-wrapper'>
                <div class='header'>
                    <div class='header-pattern'></div>
                    <h1>Xác nhận thanh toán</h1>
                </div>
                
                <div class='content'>
                    <p class='greeting'>Xin chào <strong>{customerName}</strong>,</p>
                    
                    <p>Chúng tôi xin thông báo rằng thanh toán cho đơn hàng của bạn đã được xác nhận thành công.</p>
                    
                    <div class='payment-details'>
                        <h3>Chi tiết thanh toán</h3>
                        <p>Mã đơn hàng: <strong>#{orderId}</strong></p>
                        <p>Số tiền thanh toán: <strong>${paymentAmount:N2}</strong></p>
                        <p>Phương thức thanh toán: <strong>{paymentMethod}</strong></p>
                        <p>Thời gian thanh toán: <strong>{DateTime.Now:dd/MM/yyyy HH:mm:ss}</strong></p>
                    </div>
                    
                    <p>Đơn hàng của bạn đang được xử lý và sẽ được giao trong thời gian sớm nhất.</p>
                    <p>Bạn có thể theo dõi trạng thái đơn hàng trong tài khoản của mình trên website của chúng tôi.</p>
                    <p>Cảm ơn bạn đã mua sắm tại CareSkin Beauty Shop!</p>
                    
                    <p>Trân trọng,</p>
                    <p><strong>Đội ngũ CareSkin Beauty Shop</strong></p>
                </div>
                
                <div class='footer'>
                    <p>© {DateTime.Now.Year} CareSkin Beauty Shop. All rights reserved.</p>
                    <p>Email này được gửi tự động, vui lòng không trả lời.</p>
                </div>
            </div>
        </div>
    </body>
    </html>";

            await SendEmailAsync(toEmail, subject, body);
        }


    }
}
