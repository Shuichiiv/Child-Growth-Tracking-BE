using System.Linq.Expressions;
using AutoMapper;
using DataObjects_BE;
using DataObjects_BE.Entities;
using DTOs_BE.DoctorDTOs;
using Microsoft.EntityFrameworkCore;
using Repositories_BE.Interfaces;

namespace Repositories_BE.Repositories
{
    public class ReportRepository : GenericRepository<Report>, IReportRepository 
    {
        
        private readonly SWP391G3DbContext _context;
        private readonly IFeedbackRepository _feedbackRepository;

        public ReportRepository(SWP391G3DbContext context, IFeedbackRepository feedbackRepository): base(context)
        {
            _context = context;
            _feedbackRepository = feedbackRepository;
        }
        
        public async Task<Report> GetByIDAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        
        public async Task<Report> CreateBMIReportAsync(Guid childId, double height, double weight)
        {
            double bmi = weight / Math.Pow(height / 100, 2); // Chuyển cm -> m

            var newReport = new Report
            {
                ReportId = Guid.NewGuid(),
                ChildId = childId,
                Height = height,
                Weight = weight,
                BMI = bmi,
                ReprotCreateDate = DateTime.UtcNow,
                ReportIsActive = "Active",
                ReportName = "BMI Report",
                ReportMark = GetBMICategory(bmi),
                ReportContent = $"Chỉ số BMI: {bmi:F2} - Phân loại: {GetBMICategory(bmi)}"
            };

            await _context.Reports.AddAsync(newReport);
            await _context.SaveChangesAsync();
            return newReport;
        }
        public async Task<Report> CreateReportAsync2(Guid childId, CreateReportDto dto)
        {
            var report = new Report
            {
                ChildId = childId,
                Height = dto.Height,
                Weight = dto.Weight,
                ReprotCreateDate = dto.Date
            };
        
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<IEnumerable<Report>> GetReportsByChildIdAsync(Guid childId)
        {
            return await _context.Reports
                .Where(r => r.ChildId == childId)
                .OrderByDescending(r => r.ReprotCreateDate)
                .ToListAsync();
        }

        public async Task<Report> CreateBMIReportAsync(Report report)
        {
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
            return report;
        }

        private string GetBMICategory(double bmi)
        {
            if (bmi < 16) return "Gầy độ III";
            if (bmi < 17) return "Gầy độ II";
            if (bmi < 18.5) return "Gầy độ I";
            if (bmi < 25) return "Bình thường";
            if (bmi < 30) return "Thừa cân";
            if (bmi < 35) return "Béo phì độ I";
            if (bmi < 40) return "Béo phì độ II";
            return "Béo phì độ III";
        }
        
        public async Task<Report> CreateReportAsync(Report report)
        {
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<bool> UpdateReportAsync(Report report)
        {
            _context.Reports.Update(report);
            return await _context.SaveChangesAsync() > 0;
        }

        public IEnumerable<Report> Get(Expression<Func<Report, bool>>? filter = null, Func<IQueryable<Report>, IOrderedQueryable<Report>>? orderBy = null, string includeProperties = "", int? pageIndex = null,
            int? pageSize = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ReportDto>> GetReportsByStatusAsync(string status)
        {
            return await _context.Reports
                .Where(r => r.ReportIsActive == status)
                .Select(r => new ReportDto
                {
                    ReportId = r.ReportId,
                    ChildId = r.ChildId,
                    Height = r.Height,
                    Weight = r.Weight,
                    BMI = r.BMI,
                    ReportIsActive = r.ReportIsActive,
                })
                .ToListAsync();
        }
        
        public async Task<bool> DeleteAsync(Report report)
        {
            _context.Reports.Remove(report);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<Report> GetLatestReportByIdAsync(Guid childId)
        {
            return await _context.Reports
                .Where(r => r.ChildId == childId)
                .OrderByDescending(r => r.ReprotCreateDate)
                .FirstOrDefaultAsync();
        }

    }
}