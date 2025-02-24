using DataObjects_BE.Entities;
using DTOs_BE.UserDTOs;

namespace Services_BE.Interfaces;

public interface IParentService
{
    Task<IEnumerable<Parent>> GetAllParentsAsync();
    Task<Parent> GetParentByIdAsync(Guid parentId);
    Task<bool> CreateParentAsync(ParentDto parentDto);
    Task<bool> UpdateParentAsync(Guid parentId, ParentDto parentDto);
    Task<bool> DeleteParentAsync(Guid parentId);
}