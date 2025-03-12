using DataObjects_BE.Entities;
using DTOs_BE.UserDTOs;
using Repositories_BE.Repositories;

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
        
        Task<ParentDto2> GetParentByChildIdAsync1(Guid childId);
        Task<ChildDto> GetChildByIdAsync1(Guid childId);
        
    }
}