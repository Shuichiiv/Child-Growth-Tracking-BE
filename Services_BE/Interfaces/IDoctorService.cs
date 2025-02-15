using DTOs_BE.DoctorDTOs;

namespace Services_BE.Interfaces
{
    public interface IDoctorService
    {
        Task<DoctorDto?> GetDoctorInfoAsync(Guid accountId);
        Task<bool> UpdateDoctorInfoAsync(Guid accountId, DoctorDto doctorDto);
        Task<bool> ChangePasswordAsync(Guid accountId, string oldPassword, string newPassword);
    }
}

