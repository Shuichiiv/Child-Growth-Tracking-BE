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
                return BadRequest("Incorrect old password or account not found");

            return Ok("Password changed successfully");
        }
    }
}