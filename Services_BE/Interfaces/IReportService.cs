using DTOs_BE.DoctorDTOs;

namespace Services_BE.Interfaces
{
    public interface IReportService
    {
        Task<ReportDto> CreateBMIReportAsync(Guid childId, double height, double weight);
        Task<IEnumerable<ReportDto>> GetReportsByChildIdAsync(Guid childId);
    }
}
