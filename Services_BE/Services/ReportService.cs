using DTOs_BE.DoctorDTOs;
using Repositories_BE.Interfaces;
using Services_BE.Interfaces;

namespace Services_BE.Services
{
    public class ReportService: IReportService
    {
        private readonly IReportRepository _reportRepository;
        
        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        public async Task<ReportDto> CreateBMIReportAsync(Guid childId, double height, double weight)
        {
            var report = await _reportRepository.CreateBMIReportAsync(childId, height, weight);
            return new ReportDto
            {
                ReportId = report.ReportId,
                ChildId = report.ChildId,
                Height = report.Height,
                Weight = report.Weight,
                BMI = report.BMI,
                ReportContent = report.ReportContent,
                ReportMark = report.ReportMark
            };
        }

        public async Task<IEnumerable<ReportDto>> GetReportsByChildIdAsync(Guid childId)
        {
            var reports = await _reportRepository.GetReportsByChildIdAsync(childId);
            return reports.Select(r => new ReportDto
            {
                ReportId = r.ReportId,
                ChildId = r.ChildId,
                Height = r.Height,
                Weight = r.Weight,
                BMI = r.BMI,
                ReportContent = r.ReportContent,
                ReportMark = r.ReportMark
            }).ToList();
        }
    }
}