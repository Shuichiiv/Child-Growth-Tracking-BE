using DataObjects_BE.Entities;
using DTOs_BE.DoctorDTOs;

namespace Repositories_BE.Interfaces
{
    public interface IDoctorRepository: IGenericRepository<Doctor>
    {
        Task<DoctorDto?> GetDoctorInfoAsync(Guid accountId);
        Task<bool> UpdateDoctorInfoAsync(Guid accountId, DoctorDto doctorDto);
        Task<bool> ChangePasswordAsync(Guid accountId, string oldPassword, string newPassword);
    }
}

