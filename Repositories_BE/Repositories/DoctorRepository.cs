using DataObjects_BE;
using DataObjects_BE.Entities;
using DTOs_BE.DoctorDTOs;
using Microsoft.EntityFrameworkCore;
using Repositories_BE.Interfaces;

namespace Repositories_BE.Repositories
{
    public class DoctorRepository: GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly SWP391G3DbContext _context;
        
        public DoctorRepository(SWP391G3DbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<DoctorDto?> GetDoctorInfoAsync(Guid accountId)
        {
            var result = _context.Doctors
                .Join(_context.Accounts,
                    d => d.AccountId,
                    a => a.AccountId,
                    (d, a) => new DoctorDto
                    {
                        DoctorId = d.DoctorId,
                        AccountId = a.AccountId,
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                        Email = a.Email,
                        PhoneNumber = a.PhoneNumber,
                        Specialization = d.Specialization,
                        ExperienceYears = d.ExperienceYears,
                        HospitalAddressWork = d.HospitalAddressWork,
                        ImageUrl = a.ImageUrl
                    })
                .Where(dto => dto.AccountId == accountId)
                .AsQueryable();

            // Tính StarRating từ bảng Rating thông qua Feedback
            var starRating = await _context.Ratings
                .Where(r => _context.Feedbacks
                    .Any(f => f.FeedbackId == r.FeedbackId && f.DoctorId == accountId))
                .Select(r => (double?)r.RatingValue)
                .AverageAsync() ?? 0;

            // Lấy thông tin bác sĩ
            var doctor = await result.FirstOrDefaultAsync(); 
            if (doctor != null)
            {
                doctor.StarRating = starRating;
            }

            return doctor;
        }


        public async Task<bool> UpdateDoctorInfoAsync(Guid accountId, DoctorDto doctorDto)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.AccountId == accountId);

            if (account == null || doctor == null) return false;

            account.FirstName = doctorDto.FirstName;
            account.LastName = doctorDto.LastName;
            account.Email = doctorDto.Email;
            account.PhoneNumber = doctorDto.PhoneNumber;
            account.ImageUrl = doctorDto.ImageUrl;

            doctor.Specialization = doctorDto.Specialization;
            doctor.ExperienceYears = doctorDto.ExperienceYears;
            doctor.HospitalAddressWork = doctorDto.HospitalAddressWork;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(Guid accountId, string oldPassword, string newPassword)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null || account.Password != oldPassword) return false;

            account.Password = newPassword; // Thực tế cần mã hóa password
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

