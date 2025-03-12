using DTOs_BE.AppointmentDtos;
using DTOs_BE.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services_BE.Interfaces;

namespace WebAPI_BE.Controllers
{
    [Route("api/Parent")]
    [ApiController]
    
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
        
        [HttpGet("parent-info/{childId}")]
        public async Task<IActionResult> GetParentByChildId(Guid childId)
        {
            var parent = await _childService.GetParentByChildIdAsync1(childId);
            if (parent == null) return NotFound("Parent not found");
            return Ok(parent);
        }

        [HttpGet("child-info/{childId}")]
        public async Task<IActionResult> GetChildById(Guid childId)
        {
            var child = await _childService.GetChildByIdAsync1(childId);
            if (child == null) return NotFound("Child not found");
            return Ok(child);
        }
        
        [HttpPut("children/{childId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdateChild(Guid childId, [FromBody] ChildDto childDto)
        {
            var result = await _childService.UpdateChildAsync(childId, childDto);
            if (!result) return NotFound("Không thể cập nhật thông tin trẻ.");
            return Ok("Thông tin trẻ đã được cập nhật.");
        }
        [HttpGet("{appointmentId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetAppointmentById(Guid appointmentId)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(appointmentId);
            if (appointment == null) return NotFound("Lịch hẹn không tồn tại.");
            return Ok(appointment);
        }
        
        [HttpGet("search")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> SearchChildren([FromQuery] Guid parentId, [FromQuery] string keyword)
        {
            if (parentId == Guid.Empty)
            {
                return BadRequest("ParentId is required.");
            }

            var children = await _childService.SearchChildrenAsync(parentId, keyword);
            return Ok(children);
        }
        [HttpPost("appointments/create")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentCreateDto appointmentDto)
        {
            var result = await _appointmentService.CreateAppointmentAsync(appointmentDto);
            if (!result) return BadRequest("Không thể tạo lịch hẹn.");
            return Ok("Lịch hẹn đã được tạo thành công.");
        }
        [HttpGet("{childId}/parent/{parentId}")]
        [Authorize(Roles = "User,Admin,Doctor")]
        public async Task<IActionResult> GetChildByParent(Guid childId, Guid parentId)
        {
            var child = await _childService.GetChildByIdAndParentAsync(childId, parentId);
    
            if (child == null)
                return NotFound("Không tìm thấy thông tin trẻ hoặc bạn không có quyền truy cập.");

            return Ok(child);
        }
        [HttpGet("child/{childId}")]
        [Authorize(Roles = "User,Admin,Doctor")]
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
        [Authorize(Roles = "User,Admin,Doctor")]
        public async Task<IActionResult> GetAllchildsByParentId(Guid parentId)
        {
            var children = await _parentService.GetAllChildrenByParentIdAsync(parentId);
            if (children == null || !children.Any()) return NotFound("Không tìm thấy trẻ nào.");
            return Ok(children);
        }
        
        [HttpGet("parents/{parentId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetParentById(Guid parentId)
        {
            var parent = await _parentService.GetParentByIdAsync(parentId);
            if (parent == null) return NotFound("Phụ huynh không tồn tại.");
            return Ok(parent);
        }
        
        [HttpPost("children")]
        //[Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> CreateChild([FromBody] ChildDtoCreate childDto)
        {
            try
            {
                var result = await _childService.CreateChildAsync(childDto);
                return Ok("Trẻ đã được tạo.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Trả về lỗi cụ thể từ service
            }
            catch (Exception)
            {
                return StatusCode(500, "Có lỗi xảy ra trong quá trình xử lý.");
            }
        }
        
        [HttpGet("by-accountId/{accountId}")]
        [Authorize(Roles = "User,Admin,Doctor")]
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
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> CreateParent([FromBody] ParentDto parentDto)
        {
            var result = await _parentService.CreateParentAsync(parentDto);
            if (!result) return BadRequest("Không thể tạo phụ huynh.");
            return Ok("Phụ huynh đã được tạo.");
        }

        [HttpPut("parents/{parentId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdateParent(Guid parentId, [FromBody] ParentDto parentDto)
        {
            var result = await _parentService.UpdateParentAsync(parentId, parentDto);
            if (!result) return NotFound("Không thể cập nhật phụ huynh.");
            return Ok("Thông tin phụ huynh đã được cập nhật.");
        }

        [HttpDelete("parents/{parentId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> DeleteParent(Guid parentId)
        {
            var result = await _parentService.DeleteParentAsync(parentId);
            if (!result) return NotFound("Không thể xóa phụ huynh.");
            return Ok("Phụ huynh đã được xóa.");
        }
    }
}