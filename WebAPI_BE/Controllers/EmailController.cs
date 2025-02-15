using DTOs_BE.DoctorDTOs;
using Microsoft.AspNetCore.Mvc;
using Services_BE.Interfaces;

namespace WebAPI_BE.Controllers
{
        [Route("api/email")]
        [ApiController]
        public class EmailController : ControllerBase
        {
            private readonly IEmailService _emailService;

            public EmailController(IEmailService emailService)
            {
                _emailService = emailService;
            }

            [HttpPost("send-feedback")]
            public async Task<IActionResult> SendFeedback([FromBody] EmailDto emailDto)
            {
                if (emailDto == null)
                    return BadRequest("Dữ liệu không hợp lệ.");

                var result = await _emailService.SendFeedbackEmailAsync(emailDto);

                if (result)
                    return Ok("Gửi email thành công!");

                return StatusCode(500, "Gửi email thất bại.");
            }
        }
}