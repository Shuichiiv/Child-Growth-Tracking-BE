using DTOs_BE.AppointmentDtos;
using Microsoft.AspNetCore.Mvc;
using Services_BE.Interfaces;

namespace WebAPI_BE.Controllers
{
    [Route("api/Appointment")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            return Ok(appointments);
        }
        
        [HttpGet("{appointmentId}")]
        public async Task<IActionResult> GetAppointmentById(Guid appointmentId)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(appointmentId);
            if (appointment == null) return NotFound("Lịch hẹn không tồn tại.");
            return Ok(appointment);
        }
        
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetAppointmentsByDoctor(Guid doctorId)
        {
            var appointments = await _appointmentService.GetAppointmentsByDoctorIdAsync(doctorId);
            return Ok(appointments);
        }
        
        [HttpGet("parent/{parentId}")]
        public async Task<IActionResult> GetAppointmentsByParent(Guid parentId)
        {
            var appointments = await _appointmentService.GetAppointmentsByParentIdAsync(parentId);
            return Ok(appointments);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentCreateDto appointmentDto)
        {
            var result = await _appointmentService.CreateAppointmentAsync(appointmentDto);
            if (!result) return BadRequest("Không thể tạo lịch hẹn.");
            return Ok("Lịch hẹn đã được tạo thành công.");
        }

        [HttpPut("{appointmentId}")]
        public async Task<IActionResult> UpdateAppointment(Guid appointmentId,
            [FromBody] AppointmentUpdateDto appointmentDto)
        {
            var result = await _appointmentService.UpdateAppointmentAsync(appointmentId, appointmentDto);
            if (!result) return NotFound("Lịch hẹn không tồn tại hoặc không thể cập nhật.");
            return Ok("Lịch hẹn đã được cập nhật.");
        }
        
        [HttpDelete("{appointmentId}")]
        public async Task<IActionResult> DeleteAppointment(Guid appointmentId)
        {
            var result = await _appointmentService.DeleteAppointmentAsync(appointmentId);
            if (!result) return NotFound("Lịch hẹn không tồn tại hoặc không thể xóa.");
            return Ok("Lịch hẹn đã được xóa.");
        }
    }
}