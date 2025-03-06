using DTOs_BE.DoctorDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services_BE.Interfaces;

namespace WebAPI_BE.DoctorController
{

    [Route("api/[controller]")]
    [ApiController]
    
    public class DoctorController: ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }
        [Authorize(Roles = "Doctor")]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();
            return Ok(doctors);
        }
        [Authorize(Roles = "Doctor")]
        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetDoctorInfo(Guid accountId)
        {
            var doctor = await _doctorService.GetDoctorInfoAsync(accountId);
            if (doctor == null)
                return NotFound("Doctor not found");

            return Ok(doctor);
        }
        [Authorize(Roles = "Doctor")]
        [HttpPut("{accountId}")]
        public async Task<IActionResult> UpdateDoctorInfo(Guid accountId, [FromBody] DoctorDto doctorDto)
        {
            var result = await _doctorService.UpdateDoctorInfoAsync(accountId, doctorDto);
            if (!result)
                return NotFound("Doctor not found");

            return Ok("Doctor updated successfully");
        }
        [Authorize(Roles = "Doctor")]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> SearchDoctors([FromQuery] string keyword)
        {
            var doctors = await _doctorService.SearchDoctorsAsync(keyword);
            return Ok(doctors);
        }
        [Authorize(Roles = "Doctor")]
        [HttpGet("specialty/{specialty}")]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctorsBySpecialty(string specialty)
        {
            var doctors = await _doctorService.GetDoctorsBySpecialtyAsync(specialty);
            return Ok(doctors);
        }
        [Authorize(Roles = "Doctor")]
        [HttpGet("count")]
        public async Task<ActionResult<int>> CountDoctors()
        {
            var count = await _doctorService.CountDoctorsAsync();
            return Ok(count);
        }
        [Authorize(Roles ="User")]
        [HttpGet("list-doctors-for-user")]
        public async Task<IActionResult> GetDoctorsforUser()
        {
            var response = await _doctorService.GetListDoctorsForCustomer();
            if(response == null)
                return NotFound("No doctors found");
            return Ok(response);
        }
    }
}