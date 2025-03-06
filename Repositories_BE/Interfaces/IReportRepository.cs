using DataObjects_BE.Entities;
using DTOs_BE.DoctorDTOs;

namespace Repositories_BE.Interfaces
{
    public interface IReportRepository: IGenericRepository<Report>
    {
        Task<Report> CreateBMIReportAsync(Guid childId, double height, double weight);
        Task<IEnumerable<Report>> GetReportsByChildIdAsync(Guid childId);
        Task<Report> CreateBMIReportAsync(Report report);
        Task<Report> CreateReportAsync(Report report);
        Task<Report> CreateReportAsync2(Guid childId, CreateReportDto dto);
        Task<bool> UpdateReportAsync(Report report);
    }

}