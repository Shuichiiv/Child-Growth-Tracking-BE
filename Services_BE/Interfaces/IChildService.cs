using DataObjects_BE.Entities;
using DTOs_BE.UserDTOs;

namespace Services_BE.Interfaces;

public interface IChildService
{
    Task<IEnumerable<Child>> GetAllChildrenAsync();
    Task<Child> GetChildByIdAsync(Guid childId);
    Task<bool> CreateChildAsync(ChildDtoCreate childDto);
    Task<bool> UpdateChildAsync(Guid childId, ChildDto childDto);
    Task<bool> DeleteChildAsync(Guid childId);
    Task<IEnumerable<ChildDto>> SearchChildrenAsync(Guid parentId, string keyword);
    Task<ChildDto> GetChildByIdAndParentAsync(Guid childId, Guid parentId);
}