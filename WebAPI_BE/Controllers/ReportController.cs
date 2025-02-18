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
    }
    
}