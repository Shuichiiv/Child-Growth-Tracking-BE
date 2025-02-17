using AutoMapper;
using DTOs_BE.UserDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories_BE.Interfaces;
using Services_BE.Services;
using Services_BE.Interfaces;

namespace WebAPI_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterModel registerModel)
        {
            var result = await _userService.Register(registerModel);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginModel loginModel)
        {
            var result = await _userService.LoginByEmailAndPassword(loginModel);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        
        [HttpPost("activate-account")]
        public async Task<IActionResult> ActivateAccount([FromBody] VerifyOtpModel otpModel)
        {
            if (otpModel == null)
                return BadRequest("Invalid OTP details");

            var result = await _userService.ActivateAccountAsync(otpModel);
            if (!result)
                return BadRequest("Account activation failed");

            return Ok("Account activated successfully");
        }
        
       
    }
}
