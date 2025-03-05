using DTOs_BE.DoctorDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services_BE.Interfaces;

namespace WebAPI_BE.DoctorController
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Doctor")] 
    public class DoctorController: ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }
        
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();
            return Ok(doctors);
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
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> SearchDoctors([FromQuery] string keyword)
        {
            var doctors = await _doctorService.SearchDoctorsAsync(keyword);
            return Ok(doctors);
        }

        [HttpGet("specialty/{specialty}")]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctorsBySpecialty(string specialty)
        {
            var doctors = await _doctorService.GetDoctorsBySpecialtyAsync(specialty);
            return Ok(doctors);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> CountDoctors()
        {
            var count = await _doctorService.CountDoctorsAsync();
            return Ok(count);
        }
    }
}