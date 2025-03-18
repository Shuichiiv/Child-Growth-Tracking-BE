using DataObjects_BE.Entities;
using DTOs_BE.UserDTOs;
using Repositories_BE.Repositories;

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
    


    Task<ParentDto2> GetParentByChildIdAsync1(Guid childId);
    Task<ChildDto> GetChildByIdAsync1(Guid childId);
}