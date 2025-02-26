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
        try
        {
            return await _childRepository.GetAllChildrenAsync();
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while getting reports", e);
        }
    }

    public async Task<Child> GetChildByIdAsync(Guid childId)
    {
        try
        {
            return await _childRepository.GetChildByIdAsync(childId);
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while getting reports", e);
        }
       
    }

    public async Task<bool> CreateChildAsync(ChildDto childDto)
    {
        try
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
        catch (Exception e)
        {
            throw new Exception("An error occurred while adding new child", e);
        }
        
    }

    public async Task<bool> UpdateChildAsync(Guid childId, ChildDto childDto)
    {
        try
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
        catch (Exception e)
        {
            throw new Exception("An error occurred while updating child", e);
        }
      
    }


    public async Task<bool> DeleteChildAsync(Guid childId)
    {
        try
        {
            return await _childRepository.DeleteChildAsync(childId);
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while deleting child", e);
        }
       
    }
}