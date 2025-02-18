using DataObjects_BE;
using DataObjects_BE.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories_BE.Interfaces;

namespace Repositories_BE.Repositories
{
    public class ReportRepository : IReportRepository
    {
        
        private readonly SWP391G3DbContext _context;

        public ReportRepository(SWP391G3DbContext context)
        {
            _context = context;
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

        public async Task<IEnumerable<Report>> GetReportsByChildIdAsync(Guid childId)
        {
            return await _context.Reports
                .Where(r => r.ChildId == childId)
                .OrderByDescending(r => r.ReprotCreateDate)
                .ToListAsync();
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
    }
}