using DataObjects_BE.Entities;
using DTOs_BE.UserDTOs;

namespace Services_BE.Interfaces;

public interface IChildService
{
    Task<IEnumerable<Child>> GetAllChildrenAsync();
    Task<Child> GetChildByIdAsync(Guid childId);
    Task<bool> CreateChildAsync(ChildDto childDto);
    Task<bool> UpdateChildAsync(Guid childId, ChildDto childDto);
    Task<bool> DeleteChildAsync(Guid childId);
}