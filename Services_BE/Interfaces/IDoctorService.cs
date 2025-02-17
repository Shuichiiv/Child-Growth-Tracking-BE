using DTOs_BE.DoctorDTOs;

namespace Services_BE.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync();
        Task<DoctorDto?> GetDoctorInfoAsync(Guid accountId);
        Task<bool> UpdateDoctorInfoAsync(Guid accountId, DoctorDto doctorDto);
        Task<bool> ChangePasswordAsync(Guid accountId, string oldPassword, string newPassword);
        Task<IEnumerable<DoctorDto>> SearchDoctorsAsync(string keyword);
        Task<IEnumerable<DoctorDto>> GetDoctorsBySpecialtyAsync(string specialty);
        Task<bool> DeleteDoctorAsync(Guid accountId);
        Task<int> CountDoctorsAsync();
    }
}

