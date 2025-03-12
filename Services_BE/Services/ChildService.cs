using DataObjects_BE.Entities;
using DTOs_BE.UserDTOs;
using Repositories_BE.Interfaces;
using Repositories_BE.Repositories;
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
    
    public async Task<ParentDto2> GetParentByChildIdAsync1(Guid childId)
    {
        return await _childRepository.GetParentByChildIdAsync1(childId);
    }
        
    public async Task<ChildDto> GetChildByIdAsync1(Guid childId)
    {
        return await _childRepository.GetChildByIdAsync1(childId);
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

    public async Task<bool> CreateChildAsync(ChildDtoCreate childDto)
    {
        try
        {
            if (childDto.DOB > DateTime.UtcNow)
            {
                throw new ArgumentException("Ngày sinh không thể ở tương lai.");
            }
            var child = new Child
            {
                ChildId = Guid.NewGuid(),
                ParentId = childDto.ParentId,
                FirstName = childDto.FirstName,
                LastName = childDto.LastName,
                Gender = childDto.Gender,
                DOB = DateTime.SpecifyKind(childDto.DOB, DateTimeKind.Utc),
                DateCreateAt = DateTime.UtcNow,
                DateUpdateAt = DateTime.UtcNow,
                ImageUrl = childDto.ImageUrl
            };
            return await _childRepository.CreateChildAsync(child);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Validation Error: {ex.Message}");
            return false;
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
    public async Task<IEnumerable<ChildDto>> SearchChildrenAsync(Guid parentId, string keyword)
    {
        var children = await _childRepository.SearchChildrenAsync(parentId, keyword);
        return children.Select(child => new ChildDto
        {
            ChildId = child.ChildId,
            FirstName = child.FirstName,
            ParentId = child.ParentId
        }).ToList();
    }
    public async Task<ChildDto> GetChildByIdAndParentAsync(Guid childId, Guid parentId)
    {
        var child = await _childRepository.GetChildByIdAndParentAsync(childId, parentId);
    
        if (child == null)
            return null;

        return new ChildDto
        {
            ChildId = child.ChildId,
            ParentId = child.ParentId,
            FirstName = child.FirstName,
            LastName = child.LastName,
            Gender = child.Gender,
            ImageUrl = child.ImageUrl,
            DOB = child.DOB
        };
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