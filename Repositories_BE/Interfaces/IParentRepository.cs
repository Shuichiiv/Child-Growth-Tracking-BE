using DataObjects_BE.Entities;

namespace Repositories_BE.Interfaces
{
    public interface IParentRepository : IGenericRepository<Parent>
    {
        Task<IEnumerable<Parent>> GetAllParentsAsync();
        Task<Parent> GetParentByIdAsync(Guid parentId);
        Task<bool> CreateParentAsync(Parent parent);
        Task<bool> UpdateParentAsync(Parent parent);
        Task<bool> DeleteParentAsync(Guid parentId);

        Task<IEnumerable<Child>> GetAllChildrenByParentIdAsync(Guid parentId);
        Task<Parent> GetParentByAccountId(Guid accountId);

        Task<bool> CreateParent(Parent parent);
        Task<Child> GetChildByIdAsync(Guid childId);
        Task<bool> DeleteChildByIdAsync(Guid childId);
    }
}