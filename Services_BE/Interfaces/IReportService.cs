using DataObjects_BE.Entities;
using DTOs_BE.DoctorDTOs;

namespace Services_BE.Interfaces
{
    public interface IReportService
    {
        Task<ReportDto> CreateBMIReportAsync(Guid childId, double height, double weight);
        Task<IEnumerable<ReportDto>> GetReportsByChildIdAsync(Guid childId);
        
        Task<ReportDto> CreateCustomBMIReportAsync(ReportDtoFParents reportDto);
        Task<ReportDto> CreateReportAsync(CreateReportDto request);
        Task<bool> UpdateReportAsync(Guid reportId, UpdateReportDto request);
        
        Task<Report> CreateReportAsync2(Guid childId, CreateReportDto dto);
    }
}
