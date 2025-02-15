using DTOs_BE.DoctorDTOs;
using Microsoft.AspNetCore.Mvc;
using Services_BE.Interfaces;

namespace WebAPI_BE.DoctorController
{

    public class DoctorController: ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetDoctorInfo(Guid accountId)
        {
            var doctor = await _doctorService.GetDoctorInfoAsync(accountId);
            if (doctor == null)
                return NotFound("Doctor not found");

            return Ok(doctor);
        }

        [HttpPut("{accountId}")]
        public async Task<IActionResult> UpdateDoctorInfo(Guid accountId, [FromBody] DoctorDto doctorDto)
        {
            var result = await _doctorService.UpdateDoctorInfoAsync(accountId, doctorDto);
            if (!result)
                return NotFound("Doctor not found");

            return Ok("Doctor updated successfully");
        }

        [HttpPost("change-password/{accountId}")]
        public async Task<IActionResult> ChangePassword(Guid accountId, [FromBody] ChangePasswordDto model)
        {
            var result = await _doctorService.ChangePasswordAsync(accountId, model.OldPassword, model.NewPassword);
            if (!result)
                return BadRequest("Incorrect old password or account not found");

            return Ok("Password changed successfully");
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok("Logged out successfully");
        }
    }
}