using DTOs_BE.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services_BE.Interfaces;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Repositories_BE.Interfaces;

namespace WebAPI_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService, IUserRepository userRepository)
        {
            _userService = userService;
        }

        [HttpPost("change-password/{accountId}")]
        public async Task<IActionResult> ChangePassword(Guid accountId, [FromBody] ChangePasswordModel model)
        {
            var result = await _userService.ChangePasswordAsync(accountId, model);
            if (!result)
                return BadRequest("Không tìm thấy mật khẩu cũ hoặc tài khoản không đúng!!!");

            return Ok("Mật khẩu đã được thay đổi thành công.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var result = await _userService.ResetPasswordAsync(model);
            if (!result)
                return BadRequest("Yêu cầu đặt lại mật khẩu không hợp lệ hoặc đã hết hạn!!!");

            return Ok("Mật khẩu đã được đặt lại thành công.");
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] ResendOtpRequestModel model)
        {
            var result = await _userService.RequestPasswordResetAsync(model.Email);
            if (!result)
                return BadRequest("Email không tồn tại trong hệ thống!!!");

            return Ok("Yêu cầu đặt lại mật khẩu đã được gửi.");
        }

    }
}