using DataObjects_BE.Entities;
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
            try
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
            catch (Exception e)
            {
                throw new Exception("An error occurred while cancelling appointments", e);
            }
            
        }

        public async Task<IEnumerable<ReportDto>> GetReportsByChildIdAsync(Guid childId)
        {
            try
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
            catch (Exception e)
            {
                throw new Exception("An error occurred while getting reports", e);
            }
           
        }

        public async Task<ReportDto> CreateCustomBMIReportAsync(ReportDtoFParents reportDto)
        {
            var report = new Report
            {
                ReportId = Guid.NewGuid(),
                ChildId = reportDto.ChildId,
                Height = reportDto.Height,
                Weight = reportDto.Weight,
                BMI = reportDto.Weight / ((reportDto.Height / 100) * (reportDto.Height / 100)), // BMI = W / H^2
                ReprotCreateDate = reportDto.ReportCreateDate,
                ReportIsActive = "Pending",
                ReportName = "BMI Report",
                ReportContent = $"BMI calculated on {reportDto.ReportCreateDate:yyyy-MM-dd}"
            };

            var createdReport = await _reportRepository.CreateBMIReportAsync(report);

            return new ReportDto
            {
                ReportId = createdReport.ReportId,
                ChildId = createdReport.ChildId,
                Height = createdReport.Height,
                Weight = createdReport.Weight,
                BMI = createdReport.BMI,
                ReportContent = createdReport.ReportContent,
                ReportMark = createdReport.ReportMark
            };
        }
        }
}