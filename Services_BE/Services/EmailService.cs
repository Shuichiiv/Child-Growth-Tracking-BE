using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DTOs_BE.DoctorDTOs;
using Microsoft.Extensions.Configuration;
using Services_BE.Interfaces;

namespace Services_BE.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
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

                using (var client = new SmtpClient(smtpHost, smtpPort))
                {
                    client.Credentials = new NetworkCredential(fromEmail, password);
                    client.EnableSsl = enableSsl;

                    string mailBody = GenerateEmailBody(emailDto);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromEmail),
                        Subject = $"[Feedback] {emailDto.Subject}",
                        Body = mailBody,
                        IsBodyHtml = true // Cho phép HTML trong email
                    };

                    mailMessage.To.Add(fromEmail); // Gửi về chính bạn

                    await client.SendMailAsync(mailMessage);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi mail: {ex.Message}");
                return false;
            }
        }

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
    }
}
