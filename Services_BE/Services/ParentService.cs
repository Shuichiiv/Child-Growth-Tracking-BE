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
        return await _parentRepository.GetAllParentsAsync();
    }

    public async Task<Parent> GetParentByIdAsync(Guid parentId)
    {
        return await _parentRepository.GetParentByIdAsync(parentId);
    }

    public async Task<bool> CreateParentAsync(ParentDto parentDto)
    {
        var parent = new Parent
        {
            ParentId = Guid.NewGuid(),
            AccountId = parentDto.AccountId
        };
        return await _parentRepository.CreateParentAsync(parent);
    }

    public async Task<bool> UpdateParentAsync(Guid parentId, ParentDto parentDto)
    {
        var parent = await _parentRepository.GetParentByIdAsync(parentId);
        if (parent == null) return false;

        parent.AccountId = parentDto.AccountId;

        return await _parentRepository.UpdateParentAsync(parent);
    }

    public async Task<bool> DeleteParentAsync(Guid parentId)
    {
        return await _parentRepository.DeleteParentAsync(parentId);
    }
}
