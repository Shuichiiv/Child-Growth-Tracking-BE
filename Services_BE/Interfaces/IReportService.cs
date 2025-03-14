using DataObjects_BE.Entities;
using DTOs_BE.DoctorDTOs;
using DTOs_BE.UserDTOs;
using ParentDto = Services_BE.Services.ParentDto;

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
        
        Task<IEnumerable<ReportDto>> GetReportsByStatusAsync(string status);
        Task<bool> UpdateReportStatusAsync(Guid reportId, string newStatus);
        
        
        Task<ChildDto> GetChildInfoByIdAsync(Guid childId);
        
        Task<List<ParentDto>> GetAllParentsAsync();

    }
}
