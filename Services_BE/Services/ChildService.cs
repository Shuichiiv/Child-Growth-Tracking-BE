using DataObjects_BE.Entities;
using DTOs_BE.UserDTOs;
using Repositories_BE.Interfaces;
using Services_BE.Interfaces;

namespace Services_BE.Services;

public class ChildService : IChildService
{
    private readonly IChildRepository _childRepository;

    public ChildService(IChildRepository childRepository)
    {
        _childRepository = childRepository;
    }

    public async Task<IEnumerable<Child>> GetAllChildrenAsync()
    {
        return await _childRepository.GetAllChildrenAsync();
    }

    public async Task<Child> GetChildByIdAsync(Guid childId)
    {
        return await _childRepository.GetChildByIdAsync(childId);
    }

    public async Task<bool> CreateChildAsync(ChildDto childDto)
    {
        var child = new Child
        {
            ChildId = Guid.NewGuid(),
            ParentId = childDto.ParentId,
            FirstName = childDto.FirstName,
            LastName = childDto.LastName,
            Gender = childDto.Gender,
            DOB = childDto.DOB,
            DateCreateAt = DateTime.UtcNow,
            DateUpdateAt = DateTime.UtcNow,
            ImageUrl = childDto.ImageUrl
        };
        return await _childRepository.CreateChildAsync(child);
    }

    public async Task<bool> UpdateChildAsync(Guid childId, ChildDto childDto)
    {
        var child = await _childRepository.GetChildByIdAsync(childId);
        if (child == null) return false;

        child.FirstName = childDto.FirstName;
        child.LastName = childDto.LastName;
        child.Gender = childDto.Gender;
        child.DOB = childDto.DOB;
        child.DateUpdateAt = DateTime.UtcNow;
        child.ImageUrl = childDto.ImageUrl;

        return await _childRepository.UpdateChildAsync(child);
    }


    public async Task<bool> DeleteChildAsync(Guid childId)
    {
        return await _childRepository.DeleteChildAsync(childId);
    }
}