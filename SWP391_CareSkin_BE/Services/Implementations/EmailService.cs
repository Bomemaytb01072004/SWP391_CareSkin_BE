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

        public async Task SendOrderConfirmationEmailAsync(string toEmail, string orderId, string customerName, decimal orderTotal)
        {
            string subject = $"CareSkin - Order Confirmation #{orderId}";

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
                    <h1>Order Confirmation</h1>
                </div>
                
                <div class='content'>
                    <p class='greeting'>Hello <strong>{customerName}</strong>,</p>
                    
                    <p>Thank you for your order at CareSkin Beauty Shop. Your order has been successfully confirmed.</p>
                    
                    <div class='order-details'>
                        <h3>Order Details</h3>
                        <p>Order ID: <strong>#{orderId}</strong></p>
                        <p>Total Amount: <strong>${orderTotal:N2}</strong></p>
                    </div>
                    
                    <p>We will process your order as soon as possible.</p>
                    
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='http://careskinbeauty.shop/order-details?orderId={orderId}' class='action-button'>View Order Details</a>
                    </div>
                </div>
                
                <div class='footer'>
                    <p>&copy; {DateTime.Now.Year} CareSkin Beauty Shop. All rights reserved.</p>
                    <p>This email was sent automatically, please do not reply.</p>
                    <div style='margin-top: 15px;'>
                        <p>Contact to us: <a href='http://careskinbeauty.shop/contact' style='color: #059669; font-weight: 600; text-decoration: underline;'>click here</a></p>
                    </div>
                </div>
            </div>
        </div>
    </body>
    </html>";

            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendPaymentConfirmationEmailAsync(string toEmail, string orderId, string customerName, decimal paymentAmount, string paymentMethod)
        {
            string subject = $"CareSkin - Payment Confirmation for Order #{orderId}";

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
                    <h1>Payment Confirmation</h1>
                </div>
                
                <div class='content'>
                    <p class='greeting'>Hello <strong>{customerName}</strong>,</p>
                    
                    <p>We are pleased to inform you that the payment for your order has been successfully confirmed.</p>
                    
                    <div class='payment-details'>
                        <h3>Payment Details</h3>
                        <p>Order ID: <strong>#{orderId}</strong></p>
                        <p>Payment Amount: <strong>${paymentAmount:N2}</strong></p>
                        <p>Payment Method: <strong>{paymentMethod}</strong></p>
                        <p>Payment Time: <strong>{DateTime.Now:dd/MM/yyyy HH:mm:ss}</strong></p>
                    </div>
                    
                    <p>Your order is being processed and will be delivered as soon as possible.</p>
                    <p>You can track your order status in your account on our website.</p>
                    <p>Thank you for shopping at CareSkin Beauty Shop!</p>
                    
                    <p>Best regards,</p>
                    <p><strong>CareSkin Beauty Shop Team</strong></p>
                    
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='http://careskinbeauty.shop/order-details?orderId={orderId}' class='action-button'>View Order Details</a>
                    </div>
                </div>
                
                <div class='footer'>
                    <p>&copy; {DateTime.Now.Year} CareSkin Beauty Shop. All rights reserved.</p>
                    <p>This email was sent automatically, please do not reply.</p>
                    <div style='margin-top: 15px;'>
                        <p>Contact to us: <a href='http://careskinbeauty.shop/contact' style='color: #059669; font-weight: 600; text-decoration: underline;'>click here</a></p>
                    </div>
                </div>
            </div>
        </div>
    </body>
    </html>";

            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendPINForResetPassword(string toEmail, string customerName, string pin)
        {
            string subject = "CareSkin - Password Reset Confirmation";
            if (customerName.Contains("No name"))
            {
                customerName = "customer";
            }
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
                            <h1>Password Reset</h1>
                        </div>
            
                        <div class='content'>
                            <p class='greeting'>Hello <strong>{customerName}</strong>,</p>
                
                            <p>We have received your request to reset your password. Please use the PIN code below to complete the password reset process:</p>
                
                            <div style='text-align: center; margin: 30px 0;'>
                                <h2 style='font-size: 24px; color: #059669; font-weight: bold;'>PIN Code: {pin}</h2>
                                <p style='font-size: 16px; color: #4b5563;'>This PIN code will expire in 15 minutes.</p>
                                <p style='font-size: 16px; color: #4b5563;'>For security reasons, please do not share this PIN code with anyone.</p>
                            </div>

                            <div style='text-align: center; margin: 30px 0;'>
                                <a href='http://careskinbeauty.shop/reset-password' class='action-button'>Reset Password</a>
                            </div>
                        </div>
            
                        <div class='footer'>
                            <p>&copy; {DateTime.Now.Year} CareSkin Beauty Shop. All rights reserved.</p>
                            <p>This email was sent automatically, please do not reply.</p>
                            <div style='margin-top: 15px;'>
                                <p>Contact to us: <a href='http://careskinbeauty.shop/contact' style='color: #059669; font-weight: 600; text-decoration: underline;'>click here</a></p>
                            </div>
                        </div>
                    </div>
                </div>
            </body>
            </html>";

            await SendEmailAsync(toEmail, subject, body);
        }
    }
}
