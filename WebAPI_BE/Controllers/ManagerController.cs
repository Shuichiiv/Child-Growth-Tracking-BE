using DTOs_BE.DoctorDTOs;
using DTOs_BE.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services_BE.Interfaces;

namespace WebAPI_BE.Controllers
{
    [Route("api/Manager")]
    [ApiController]
    [Authorize(Roles = "Manager")]
    public class ManagerController: ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IParentService _parentService;
        private readonly IChildService _childService;
        private readonly IAppointmentService _appointmentService;

        public ManagerController(
            IDoctorService doctorService,
            IParentService parentService,
            IChildService childService,
            IAppointmentService appointmentService)
        {
            _doctorService = doctorService;
            _parentService = parentService;
            _childService = childService;
            _appointmentService = appointmentService;
        }
        
        [HttpGet("doctors")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();
            return Ok(doctors);
        }

        [HttpGet("doctors/{doctorId}")]
        public async Task<IActionResult> GetDoctorById(Guid doctorId)
        {
            var doctor = await _doctorService.GetDoctorInfoAsync(doctorId);
            if (doctor == null) return NotFound("Bác sĩ không tồn tại.");
            return Ok(doctor);
        }
        
        [HttpPut("doctors/{doctorId}")]
        public async Task<IActionResult> UpdateDoctor(Guid doctorId, [FromBody] DoctorDto doctorDto)
        {
            var result = await _doctorService.UpdateDoctorInfoAsync(doctorId, doctorDto);
            if (!result) return NotFound("Không thể cập nhật bác sĩ.");
            return Ok("Thông tin bác sĩ đã được cập nhật.");
        }

        [HttpDelete("doctors/{doctorId}")]
        public async Task<IActionResult> DeleteDoctor(Guid doctorId)
        {
            var result = await _doctorService.DeleteDoctorAsync(doctorId);
            if (!result) return NotFound("Không thể xóa bác sĩ.");
            return Ok("Bác sĩ đã được xóa.");
        }

        [HttpGet("doctors/search/{keyword}")]
        public async Task<IActionResult> SearchDoctors(string keyword)
        {
            var doctors = await _doctorService.SearchDoctorsAsync(keyword);
            return Ok(doctors);
        }
        
        [HttpGet("parents")]
        public async Task<IActionResult> GetAllParents()
        {
            var parents = await _parentService.GetAllParentsAsync();
            return Ok(parents);
        }

        [HttpGet("parents/{parentId}")]
        public async Task<IActionResult> GetParentById(Guid parentId)
        {
            var parent = await _parentService.GetParentByIdAsync(parentId);
            if (parent == null) return NotFound("Phụ huynh không tồn tại.");
            return Ok(parent);
        }

        [HttpPost("parents")]
        public async Task<IActionResult> CreateParent([FromBody] ParentDto parentDto)
        {
            var result = await _parentService.CreateParentAsync(parentDto);
            if (!result) return BadRequest("Không thể tạo phụ huynh.");
            return Ok("Phụ huynh đã được tạo.");
        }

        [HttpPut("parents/{parentId}")]
        public async Task<IActionResult> UpdateParent(Guid parentId, [FromBody] ParentDto parentDto)
        {
            var result = await _parentService.UpdateParentAsync(parentId, parentDto);
            if (!result) return NotFound("Không thể cập nhật phụ huynh.");
            return Ok("Thông tin phụ huynh đã được cập nhật.");
        }

        [HttpDelete("parents/{parentId}")]
        public async Task<IActionResult> DeleteParent(Guid parentId)
        {
            var result = await _parentService.DeleteParentAsync(parentId);
            if (!result) return NotFound("Không thể xóa phụ huynh.");
            return Ok("Phụ huynh đã được xóa.");
        }

        [HttpGet("children")]
        public async Task<IActionResult> GetAllChildren()
        {
            var children = await _childService.GetAllChildrenAsync();
            return Ok(children);
        }

        [HttpGet("children/{childId}")]
        public async Task<IActionResult> GetChildById(Guid childId)
        {
            var child = await _childService.GetChildByIdAsync(childId);
            if (child == null) return NotFound("Trẻ không tồn tại.");
            return Ok(child);
        }

        [HttpPost("children")]
        public async Task<IActionResult> CreateChild([FromBody] ChildDto childDto)
        {
            var result = await _childService.CreateChildAsync(childDto);
            if (!result) return BadRequest("Không thể tạo trẻ.");
            return Ok("Trẻ đã được tạo.");
        }

        [HttpPut("children/{childId}")]
        public async Task<IActionResult> UpdateChild(Guid childId, [FromBody] ChildDto childDto)
        {
            var result = await _childService.UpdateChildAsync(childId, childDto);
            if (!result) return NotFound("Không thể cập nhật thông tin trẻ.");
            return Ok("Thông tin trẻ đã được cập nhật.");
        }

        [HttpDelete("children/{childId}")]
        public async Task<IActionResult> DeleteChild(Guid childId)
        {
            var result = await _childService.DeleteChildAsync(childId);
            if (!result) return NotFound("Không thể xóa trẻ.");
            return Ok("Trẻ đã được xóa.");
        }
        
        [HttpGet("appointments")]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            return Ok(appointments);
        }

        [HttpPut("appointments/{appointmentId}/confirm")]
        public async Task<IActionResult> ConfirmAppointment(Guid appointmentId)
        {
            var result = await _appointmentService.ConfirmAppointmentAsync(appointmentId);
            if (!result) return NotFound("Không thể xác nhận lịch hẹn.");
            return Ok("Lịch hẹn đã được xác nhận.");
        }

        [HttpPut("appointments/{appointmentId}/cancel")]
        public async Task<IActionResult> CancelAppointment(Guid appointmentId)
        {
            var result = await _appointmentService.CancelAppointmentAsync(appointmentId);
            if (!result) return NotFound("Không thể hủy lịch hẹn.");
            return Ok("Lịch hẹn đã được hủy.");
        }
    }
}