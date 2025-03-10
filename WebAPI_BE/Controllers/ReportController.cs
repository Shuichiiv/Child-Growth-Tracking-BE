using DTOs_BE.DoctorDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services_BE.Interfaces;

namespace WebAPI_BE.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost("calculate-bmi")]
        public async Task<IActionResult> CalculateBMI([FromQuery] Guid childId, [FromQuery] double height, [FromQuery] double weight)
        {
            if (childId == Guid.Empty || height <= 0 || weight <= 0)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var report = await _reportService.CreateBMIReportAsync(childId, height, weight);
            return Ok(report);
        }
        
        [HttpPost("create-kid-bmi")]
        public async Task<IActionResult> CreateCustomBMIReport([FromBody] ReportDtoFParents reportDto)
        {
            if (reportDto.ChildId == Guid.Empty || reportDto.Height <= 0 || reportDto.Weight <= 0)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var report = await _reportService.CreateCustomBMIReportAsync(reportDto);
            return Ok(report);
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
        
        [HttpGet("{childId}")]
        public async Task<IActionResult> GetReportsByChildId(Guid childId)
        {
            var reports = await _reportService.GetReportsByChildIdAsync(childId);
            if (!reports.Any()) return NotFound("Không có báo cáo nào.");
            return Ok(reports);
        }
        
        [HttpPost("{childId}")]
        public async Task<IActionResult> CreateReport(Guid childId, [FromBody] CreateReportDto dto)
        {
            if (childId == Guid.Empty || dto.Height <= 0 || dto.Weight <= 0)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }
        
            var report = await _reportService.CreateReportAsync2(childId, dto);
            return Ok(report);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] CreateReportDto request)
        {
            if (request.Height <= 0 || request.Weight <= 0)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var report = await _reportService.CreateReportAsync(request);
            return Ok(report);
        }

        [HttpPut("{reportId}")]
        public async Task<IActionResult> UpdateReport(Guid reportId, [FromBody] UpdateReportDto request)
        {
            var updated = await _reportService.UpdateReportAsync(reportId, request);
            if (!updated) return NotFound("Không tìm thấy báo cáo để cập nhật.");
            return Ok("Cập nhật thành công.");
        }
        [HttpGet("inactive")]
        public async Task<IActionResult> GetPendingReportsInactive()
        {
            var reports = await _reportService.GetReportsByStatusAsync("Inactive");
            if (reports == null || !reports.Any())
            {
                return NotFound("Không có báo cáo nào ở trạng thái Inactive.");
            }

            return Ok(reports);
        }
        
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingReportsPending()
        {
            var reports = await _reportService.GetReportsByStatusAsync("Pending");
            if (reports == null || !reports.Any())
            {
                return NotFound("Không có báo cáo nào ở trạng thái Pending.");
            }

            return Ok(reports);
        }
        [HttpGet("active")]
        public async Task<IActionResult> GetPendingReportsActive()
        {
            var reports = await _reportService.GetReportsByStatusAsync("Active");
            if (reports == null || !reports.Any())
            {
                return NotFound("Không có báo cáo nào ở trạng thái Active.");
            }

            return Ok(reports);
        }
        
        [HttpPut("{reportId}/status")]
        public async Task<IActionResult> UpdateReportStatus(Guid reportId, [FromBody] string newStatus)
        {
            if (reportId == Guid.Empty || string.IsNullOrWhiteSpace(newStatus))
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var updated = await _reportService.UpdateReportStatusAsync(reportId, newStatus);
            if (!updated) return NotFound("Không tìm thấy báo cáo để cập nhật trạng thái.");

            return Ok("Cập nhật trạng thái thành công.");
        }


    }
    
}