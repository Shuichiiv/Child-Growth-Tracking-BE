using DataObjects_BE.Entities;

namespace Repositories_BE.Interfaces
{
    public interface IReportRepository
    {
        Task<Report> CreateBMIReportAsync(Guid childId, double height, double weight);
        Task<IEnumerable<Report>> GetReportsByChildIdAsync(Guid childId);
    }

}