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

        public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync()
        {
            return await _doctorRepository.GetAllDoctorsAsync();
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
        public async Task<IEnumerable<DoctorDto>> SearchDoctorsAsync(string keyword)
        {
            return await _doctorRepository.SearchDoctorsAsync(keyword);
        }

        public async Task<IEnumerable<DoctorDto>> GetDoctorsBySpecialtyAsync(string specialty)
        {
            return await _doctorRepository.GetDoctorsBySpecialtyAsync(specialty);
        }

        public async Task<bool> DeleteDoctorAsync(Guid accountId)
        {
            return await _doctorRepository.DeleteDoctorAsync(accountId);
        }

        public async Task<int> CountDoctorsAsync()
        {
            return await _doctorRepository.CountDoctorsAsync();
        }
        public async Task<List<GetDoctorByIdForCustomerModel>> GetListDoctorsForCustomer()
        {
            try
            {
                var list = _doctorRepository.GetListDoctorsForCustomer();
                if (list == null || !list.Any())
                {
                    throw new Exception("No doctors found");
                }
                List<GetDoctorByIdForCustomerModel> listModels = new List<GetDoctorByIdForCustomerModel>();
                foreach (var d in list)
                {
                    var getdoctor = new GetDoctorByIdForCustomerModel()
                    {
                        DoctorId = d.DoctorId,
                        AccountId = d.AccountId,
                        FirstName = d.Account.FirstName,
                        LastName = d.Account.LastName
                    };
                    listModels.Add(getdoctor);
                }
                return listModels;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}