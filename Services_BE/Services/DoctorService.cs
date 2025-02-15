using DTOs_BE.DoctorDTOs;
using Repositories_BE.Interfaces;
using Services_BE.Interfaces;

namespace Services_BE.Services
{

    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<DoctorDto?> GetDoctorInfoAsync(Guid accountId)
        {
            return await _doctorRepository.GetDoctorInfoAsync(accountId);
        }

        public async Task<bool> UpdateDoctorInfoAsync(Guid accountId, DoctorDto doctorDto)
        {
            return await _doctorRepository.UpdateDoctorInfoAsync(accountId, doctorDto);
        }

        public async Task<bool> ChangePasswordAsync(Guid accountId, string oldPassword, string newPassword)
        {
            return await _doctorRepository.ChangePasswordAsync(accountId, oldPassword, newPassword);
        }
    }
}