using DTOs_BE.AppointmentDtos;
using DTOs_BE.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services_BE.Interfaces;

namespace WebAPI_BE.Controllers
{
    [Route("api/Parent")]
    [ApiController]
    [Authorize(Roles = "User,Admin")]
    public class ParentController : ControllerBase
    {
        private readonly IParentService _parentService;
        private readonly IChildService _childService;
        private readonly IAppointmentService _appointmentService;
        private readonly IReportService _reportService;
        
        public ParentController(
            IParentService parentService,
            IChildService childService,
            IReportService reportService,
            IAppointmentService appointmentService)
        {
            _parentService = parentService;
            _childService = childService;
            _reportService = reportService;
            _appointmentService = appointmentService;
        }
        
        [HttpGet("{appointmentId}")]
        public async Task<IActionResult> GetAppointmentById(Guid appointmentId)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(appointmentId);
            if (appointment == null) return NotFound("Lịch hẹn không tồn tại.");
            return Ok(appointment);
        }
        
        [HttpPost("appointments/create")]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentCreateDto appointmentDto)
        {
            var result = await _appointmentService.CreateAppointmentAsync(appointmentDto);
            if (!result) return BadRequest("Không thể tạo lịch hẹn.");
            return Ok("Lịch hẹn đã được tạo thành công.");
        }
        
        [HttpGet("child/{childId}")]
        public async Task<IActionResult> GetReportsByChild(Guid childId)
        {
            if (childId == Guid.Empty)
            {
                return BadRequest("ChildId không hợp lệ.");
            }

            var reports = await _reportService.GetReportsByChildIdAsync(childId);
            return Ok(reports);
        }
        
        [HttpGet("parents/{parentId}/children")]
        public async Task<IActionResult> GetAllchildsByParentId(Guid parentId)
        {
            var children = await _parentService.GetAllChildrenByParentIdAsync(parentId);
            if (children == null || !children.Any()) return NotFound("Không tìm thấy trẻ nào.");
            return Ok(children);
        }
        
        [HttpGet("parents/{parentId}")]
        public async Task<IActionResult> GetParentById(Guid parentId)
        {
            var parent = await _parentService.GetParentByIdAsync(parentId);
            if (parent == null) return NotFound("Phụ huynh không tồn tại.");
            return Ok(parent);
        }
        
        [HttpPost("children")]
        public async Task<IActionResult> CreateChild([FromBody] ChildDto childDto)
        {
            var result = await _childService.CreateChildAsync(childDto);
            if (!result) return BadRequest("Không thể tạo trẻ.");
            return Ok("Trẻ đã được tạo.");
        }
        
        [HttpGet("by-accountId/{accountId}")]
        public async Task<IActionResult> GetParentByAccountId(Guid accountId)
        {
            var parent = await _parentService.GetParentByAccountId(accountId);

            if (parent == null)
            {
                return NotFound(new { message = "Parent not found" });
            }

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
    }
}