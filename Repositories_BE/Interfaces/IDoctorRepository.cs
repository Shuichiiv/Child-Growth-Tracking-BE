using DataObjects_BE.Entities;
using DTOs_BE.DoctorDTOs;

namespace Repositories_BE.Interfaces
{
    public interface IDoctorRepository: IGenericRepository<Doctor>
    {
        Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync();
        Task<DoctorDto?> GetDoctorInfoAsync(Guid accountId);
        Task<bool> UpdateDoctorInfoAsync(Guid accountId, DoctorDto doctorDto);
        Task<bool> ChangePasswordAsync(Guid accountId, string oldPassword, string newPassword);
        
        // Non-Feature
        Task<IEnumerable<DoctorDto>> SearchDoctorsAsync(string keyword);
        Task<IEnumerable<DoctorDto>> GetDoctorsBySpecialtyAsync(string specialty);
        Task<bool> DeleteDoctorAsync(Guid accountId);
        Task<int> CountDoctorsAsync();
        List<Doctor> GetListDoctorsForCustomer();
    }
}

