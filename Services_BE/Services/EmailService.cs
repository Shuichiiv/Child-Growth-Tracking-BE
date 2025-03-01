using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DTOs_BE.DoctorDTOs;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Security;
using Services_BE.Interfaces;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Services_BE.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        
        public EmailService(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<bool> SendFeedbackEmailAsync(EmailDto emailDto)
        {
            try
            {
                var smtpSettings = _config.GetSection("SmtpSettings");
                string fromEmail = smtpSettings["Username"];
                string password = smtpSettings["Password"];
                string smtpHost = smtpSettings["Host"];
                int smtpPort = int.Parse(smtpSettings["Port"]);
                bool enableSsl = bool.Parse(smtpSettings["EnableSsl"]);

                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Admin", fromEmail));
                email.To.Add(new MailboxAddress("", fromEmail));
                email.Subject = $"[Feedback] {emailDto.Subject}";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = GenerateEmailBody(emailDto)
                };
                email.Body = bodyBuilder.ToMessageBody();

                using (var smtp = new SmtpClient())
                {
                    await smtp.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(fromEmail, password);
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi email: {ex.Message}");
                return false;
            }
        }
        
        private static string GenerateEmailBody(EmailDto emailDto)
        {
            return $@"
        <div style='font-family: Arial, sans-serif; padding: 20px; line-height: 1.6; background-color: #f4f8f9; border-radius: 8px;'>
            <div style='text-align: center; margin-bottom: 20px;'>
                <img src='https://cdn-icons-png.flaticon.com/512/609/609803.png' width='60' alt='Medical Icon'/>
                <h2 style='color: #2b8a3e; border-bottom: 2px solid #2b8a3e; padding-bottom: 5px;'>Thông báo góp ý Childs Tracking System</h2>
            </div>
            <p><strong>Họ tên:</strong> {emailDto.FullName}</p>
            <p><strong>Email:</strong> {emailDto.Email}</p>
            <p><strong>Tiêu đề:</strong> {emailDto.Subject}</p>
            <div style='margin-top: 15px; padding: 15px; background-color: #e6f4ea; border-left: 5px solid #2b8a3e; border-radius: 5px;'>
                <p><strong>Nội dung góp ý:</strong></p>
                <p>{emailDto.Message}</p>
            </div>
            <p style='margin-top: 20px; font-size: 14px; color: gray; text-align: center;'>Đây là email tự động, vui lòng không trả lời.</p>
        </div>";
        }

        
        
        /*
        private static string GenerateEmailBody(EmailDto emailDto)
        {
            return $@"
                <div style='font-family: Arial, sans-serif; padding: 20px; line-height: 1.6;'>
                    <h2 style='color: #007bff; border-bottom: 2px solid #007bff; padding-bottom: 5px;'>Bạn vừa nhận được một góp ý mới</h2>
                    <p><strong>Họ tên:</strong> {emailDto.FullName}</p>
                    <p><strong>Email:</strong> {emailDto.Email}</p>
                    <p><strong>Tiêu đề:</strong> {emailDto.Subject}</p>
                    <div style='margin-top: 15px; padding: 10px; background-color: #f8f9fa; border-left: 5px solid #007bff;'>
                        <p><strong>Nội dung góp ý:</strong></p>
                        <p>{emailDto.Message}</p>
                    </div>
                    <p style='margin-top: 20px; font-size: 14px; color: gray;'>Đây là email tự động, vui lòng không trả lời.</p>
                </div>";
        }
        */
        public async Task SendVerifymailAsync(string toEmail, string subject, string body)
        {
            try
            {
                if (string.IsNullOrEmpty(toEmail))
                    throw new ArgumentException("Email không hợp lệ.");
        
                if (string.IsNullOrEmpty(subject))
                    throw new ArgumentException("Subject không hợp lệ.");
        
                if (string.IsNullOrEmpty(body))
                    throw new ArgumentException("Body không hợp lệ.");

                var smtpSettings = _config.GetSection("SmtpSettings");
                if (smtpSettings == null)
                    throw new Exception("Cấu hình SMTP không tìm thấy.");
                string fromEmail = smtpSettings["Username"];
                string password = smtpSettings["Password"];
                string smtpHost = smtpSettings["Host"];
                int smtpPort = int.Parse(smtpSettings["Port"]);
                bool enableSsl = bool.Parse(smtpSettings["EnableSsl"]);

                Console.WriteLine($"SMTP Host: {smtpHost}");
                Console.WriteLine($"Sender Email: {fromEmail}");
                Console.WriteLine($"Sender Password: {password}");


                if (string.IsNullOrEmpty(fromEmail) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(smtpHost))
                    throw new Exception("Cấu hình SMTP không hợp lệ.");

                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Admin", fromEmail));
                email.To.Add(new MailboxAddress("", toEmail));
                email.Subject = subject;

                var bodyBuilder = new BodyBuilder { HtmlBody = body };
                email.Body = bodyBuilder.ToMessageBody();

                using (var smtp = new SmtpClient())
                {
                    await smtp.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(fromEmail, password);
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Argument Exception: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }


        
    }
}
