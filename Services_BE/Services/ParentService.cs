using DataObjects_BE.Entities;
using DTOs_BE.UserDTOs;
using Repositories_BE.Interfaces;
using Services_BE.Interfaces;

namespace Services_BE.Services;

public class ParentService :IParentService
{
    private readonly IParentRepository _parentRepository;

    public ParentService(IParentRepository parentRepository)
    {
        _parentRepository = parentRepository;
    }
    public async Task<IEnumerable<Parent>> GetAllParentsAsync()
    {
        try
        {
            return await _parentRepository.GetAllParentsAsync();
        }
        catch (Exception e)
        {
            throw new Exception("An error at parent service", e);
        }
    }

    public async Task<Parent> GetParentByIdAsync(Guid parentId)
    {
        try
        {
            return await _parentRepository.GetParentByIdAsync(parentId);
        }
        catch (Exception e)
        {
            throw new Exception("An error at parent service", e);
        }
    }

    public async Task<bool> CreateParentAsync(ParentDto parentDto)
    {
        try
        {
            var parent = new Parent
            {
                ParentId = Guid.NewGuid(),
                AccountId = parentDto.AccountId
            };
            return await _parentRepository.CreateParentAsync(parent);
        }
        catch (Exception e)
        {
            throw new Exception("An error at parent service", e);
        }
        
    }

    public async Task<bool> UpdateParentAsync(Guid parentId, ParentDto parentDto)
    {
        try
        {
            var parent = await _parentRepository.GetParentByIdAsync(parentId);
            if (parent == null) return false;

            parent.AccountId = parentDto.AccountId;

            return await _parentRepository.UpdateParentAsync(parent);
        }
        catch (Exception e)
        {
            throw new Exception("An error at parent service", e);
        }
    }

    public async Task<bool> DeleteParentAsync(Guid parentId)
    {
        try
        {
            return await _parentRepository.DeleteParentAsync(parentId);
        }
        catch (Exception e)
        {
            throw new Exception("An error at parent service", e);
        }
    }

    public async Task<IEnumerable<Child>> GetAllChildrenByParentIdAsync(Guid parentId)
    {
        try
        {
            return await _parentRepository.GetAllChildrenByParentIdAsync(parentId);
        }
        catch (Exception e)
        {
            throw new Exception("An error at parent service", e);
        }
       
    }
    public async Task<Parent> GetParentByAccountId(Guid accountId)
    {
        return await _parentRepository.GetParentByAccountId(accountId);
    }
}
