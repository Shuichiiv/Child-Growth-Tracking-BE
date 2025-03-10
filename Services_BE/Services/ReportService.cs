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
                    ReportMark = report.ReportMark,
                    ReportIsActive = "Inactive"
                    
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
                    ReportMark = r.ReportMark,
                    ReportIsActive = "Inactive"
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
                ReportIsActive = "Inactive",
                ReportName = "BMI Report",
                ReportContent = $"BMI calculated on {reportDto.ReportCreateDate:yyyy-MM-dd}",
                ReportMark = GetBMICategory(reportDto.Weight / ((reportDto.Height / 100) * (reportDto.Height / 100)))
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
                ReportMark = createdReport.ReportMark,
                ReportIsActive = "Inactive"
            };
        }
        private string GetBMICategory(double bmi)
        {
            if (bmi < 16.0) return "Gầy độ III (Rất gầy) - Nguy cơ cao";
            if (bmi < 16.9) return "Gầy độ II - Nguy cơ vừa";
            if (bmi < 18.4) return "Gầy độ I - Nguy cơ thấp";
            if (bmi < 24.9) return "Cân nặng bình thường - Bình thường";
            if (bmi < 29.9) return "Thừa cân - Nguy cơ tăng nhẹ";
            if (bmi < 34.9) return "Béo phì độ I - Nguy cơ trung bình";
            if (bmi < 39.9) return "Béo phì độ II - Nguy cơ cao";
            return "Béo phì độ III - Nguy cơ rất cao";
        }

        public async Task<ReportDto> CreateReportAsync(CreateReportDto request)
        {
            var report = new Report
            {
                ReportId = Guid.NewGuid(),
                Height = request.Height,
                Weight = request.Weight,
                BMI = request.Weight / Math.Pow(request.Height / 100, 2), 
                ReprotCreateDate = request.Date,
                ReportIsActive = "Inactive"
            };

            var createdReport = await _reportRepository.CreateReportAsync(report);
            return new ReportDto
            {
                ReportId = createdReport.ReportId,
                ChildId = createdReport.ChildId,
                Height = createdReport.Height,
                Weight = createdReport.Weight,
                BMI = createdReport.BMI,
                ReportCreateDate = createdReport.ReprotCreateDate,
                ReportIsActive = "Inactive"
            };
        }
        public async Task<Report> CreateReportAsync2(Guid childId, CreateReportDto dto)
        {
            return await _reportRepository.CreateReportAsync2(childId, dto);
        }

        public async Task<bool> UpdateReportAsync(Guid reportId, UpdateReportDto request)
        {
            var reports = await _reportRepository.GetReportsByChildIdAsync(request.ChildId);
            var report = reports.FirstOrDefault(r => r.ReportId == reportId);
            if (report == null) return false;

            report.Height = request.Height;
            report.Weight = request.Weight;
            report.ReprotCreateDate = request.Date;
            report.BMI = request.Weight / Math.Pow(request.Height / 100, 2);
            report.ReportMark = GetBMICategory(report.BMI);

            return await _reportRepository.UpdateReportAsync(report);           
        }
        public async Task<IEnumerable<ReportDto>> GetReportsByStatusAsync(string status)
        {
            return await _reportRepository.GetReportsByStatusAsync(status);
        }

        public async Task<bool> UpdateReportStatusAsync(Guid reportId, string newStatus)
        {
            var report = await _reportRepository.GetAsync(r => r.ReportId == reportId);
            if (report == null) return false;

            report.ReportIsActive = newStatus;
            return await _reportRepository.UpdateReportAsync(report);
        }

        }
}