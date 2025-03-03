using DataObjects_BE.Entities;

namespace Repositories_BE.Interfaces
{
    public interface IChildRepository : IGenericRepository<Child>
    {
        Task<IEnumerable<Child>> GetAllChildrenAsync();
        Task<Child> GetChildByIdAsync(Guid childId);
        Task<bool> CreateChildAsync(Child child);
        Task<bool> UpdateChildAsync(Child child);
        Task<bool> DeleteChildAsync(Guid childId);
        Task<IEnumerable<Child>> SearchChildrenAsync(Guid parentId, string keyword);
        Task<Child> GetChildByIdAndParentAsync(Guid childId, Guid parentId);
    }
}