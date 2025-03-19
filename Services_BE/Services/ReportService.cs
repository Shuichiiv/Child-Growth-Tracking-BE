using DataObjects_BE.Entities;
using DTOs_BE.DoctorDTOs;
using DTOs_BE.UserDTOs;
using Repositories_BE.Interfaces;
using Services_BE.Interfaces;

namespace Services_BE.Services
{
    public class ReportService: IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IChildRepository _childRepository;
        private readonly IParentRepository _parentRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IRatingRepository _ratingRepository;
        
        public ReportService(IReportRepository reportRepository,IRatingRepository ratingRepository, IFeedbackRepository feedbackRepository, IChildRepository childRepository, IParentRepository parentRepository)
        {
            _reportRepository = reportRepository;
            _feedbackRepository = feedbackRepository;
            _childRepository = childRepository;
            _parentRepository = parentRepository;
            _ratingRepository = ratingRepository;
        }
        public async Task<ReportDto> CreateBMIReportAsync(Guid childId, double height, double weight)
        {
            try
            {
                var latestReport = await _reportRepository.GetLatestReportByIdAsync(childId);
                
                if (latestReport != null && latestReport.ReprotCreateDate.Date == DateTime.UtcNow.Date)
                {
                    throw new Exception("Bạn đã tạo báo cáo cho trẻ trong ngày hôm nay rồi.");
                }
                var report = await _reportRepository.CreateBMIReportAsync(childId, height, weight);
                return new ReportDto
                {
                    ReportId = report.ReportId,
                    ChildId = report.ChildId,
                    Height = report.Height,
                    Weight = report.Weight,
                    BMI = report.BMI,
                    ReportContent = report.ReportContent,
                    ReportMark = report.ReportMark,
                    ReportIsActive = "Inactive"
                };
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while cancelling appointments", e);
            }
            
        }

        public async Task<ChildDto> GetChildInfoByIdAsync(Guid childId)
        {
            var child = await _childRepository.GetChildByIdAsync(childId);
            if (child == null) return null;

            return new ChildDto
            {
                ChildId = child.ChildId,
                ParentId = child.ParentId,
                FirstName = child.FirstName,
                LastName = child.LastName,
                Gender = child.Gender,
                DOB = child.DOB,
                ImageUrl = child.ImageUrl
            };
        }

        public async Task<List<ParentDto>> GetAllParentsAsync()
        {
            var parents = await _parentRepository.GetAllParentsAsync();

            return parents.Select(p => new ParentDto
            {
                AccountId = p.Account?.AccountId ?? Guid.Empty,
                FullName = p.Account != null ? $"{p.Account.FirstName} {p.Account.LastName}" : "Unknown",
                Email = p.Account?.Email ?? "No Email",
                PhoneNumber = p.Account?.PhoneNumber ?? "No Phone"
            }).ToList();
        }



        public async Task<IEnumerable<ReportDto>> GetReportsByChildIdAsync(Guid childId)
        {
            try
            {
                var reports = await _reportRepository.GetReportsByChildIdAsync(childId);
                return reports
                    .Where(r => r.ReportIsActive != "3")
                    .Select(r => new ReportDto
                {
                    ReportId = r.ReportId,
                    ChildId = r.ChildId,
                    Height = r.Height,
                    Weight = r.Weight,
                    BMI = r.BMI,
                    ReportName = r.ReportName,
                    ReportContent = r.ReportContent,
                    ReportMark = r.ReportMark,
                    ReportIsActive = r.ReportIsActive.ToString(),
                    ReportCreateDate = r.ReprotCreateDate
                }).ToList();
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while getting reports", e);
            }
           
        }

        public async Task<ReportDto> CreateCustomBMIReportAsync(ReportDtoFParents reportDto)
        {
            if (reportDto == null)
                throw new ArgumentNullException(nameof(reportDto), "Dữ liệu đầu vào không hợp lệ.");
            
            var child = await _childRepository.GetChildByIdAsync(reportDto.ChildId);
            if (child == null)
                throw new KeyNotFoundException("Không tìm thấy trẻ.");
            
            var reports = await _reportRepository.GetReportsByChildIdAsync(reportDto.ChildId) ?? new List<Report>();
            
            bool isDuplicateDate = reports.Any(r => r.ReprotCreateDate.Date == reportDto.ReportCreateDate.Date);
            if (isDuplicateDate)
                throw new InvalidOperationException("Đã tồn tại báo cáo vào ngày này.");
            
            if (reportDto.ReportCreateDate < child.DOB || reportDto.ReportCreateDate > DateTime.UtcNow)
                throw new ArgumentException("Không thể tạo báo cáo vì ngày chưa hợp lệ.");

             /*
             var child = await _childRepository.GetChildByIdAsync(reportDto.ChildId);
             
             var reports = await _reportRepository.GetReportsByChildIdAsync(reportDto.ChildId);
             var report1 = reports.FirstOrDefault(r => r.ReportId == reports.LastOrDefault().ReportId);
            
             bool isDuplicateDate = reports.Any(r => r.ReprotCreateDate.Date == reportDto.ReportCreateDate && r.ReportId != report1.ReportId);
             if (isDuplicateDate)
                 throw new Exception("Đã tồn tại báo cáo vào ngày này.");
               
                if (child == null)
                    throw new KeyNotFoundException("Khong tim thay tre");

                if (reportDto.ReportCreateDate < child.DOB && reportDto.ReportCreateDate > DateTime.UtcNow)
                    throw new Exception("Không thể tạo báo cáo vì ngày chưa hợp lệ.");
                    */

                var report = new Report
                {
                    ReportId = Guid.NewGuid(),
                    ChildId = reportDto.ChildId,
                    Height = reportDto.Height,
                    Weight = reportDto.Weight,
                    BMI = reportDto.Weight / ((reportDto.Height / 100) * (reportDto.Height / 100)),
                    ReprotCreateDate = reportDto.ReportCreateDate,
                    ReportIsActive = "2",
                    ReportName = "BMI Report",
                    ReportContent = $"BMI calculated on {reportDto.ReportCreateDate:yyyy-MM-dd}",
                    ReportMark = GetBMICategory(reportDto.Weight / ((reportDto.Height / 100) * (reportDto.Height / 100)))
                };

                var createdReport = await _reportRepository.CreateBMIReportAsync(report);
                return new ReportDto
                {
                    ReportId = createdReport.ReportId,
                    ChildId = createdReport.ChildId,
                    Height = createdReport.Height,
                    Weight = createdReport.Weight,
                    BMI = createdReport.BMI,
                    ReportName = createdReport.ReportName,
                    ReportContent = createdReport.ReportContent,
                    ReportMark = createdReport.ReportMark,
                    ReportIsActive = "2"
                };
        }
        private string GetBMICategory(double bmi)
        {
            if (bmi < 16.0) return "Gầy độ III (Rất gầy) - Nguy cơ cao";
            if (bmi < 16.9) return "Gầy độ II - Nguy cơ vừa";
            if (bmi < 18.4) return "Gầy độ I - Nguy cơ thấp";
            if (bmi < 24.9) return "Cân nặng bình thường - Bình thường";
            if (bmi < 29.9) return "Thừa cân - Nguy cơ tăng nhẹ";
            if (bmi < 34.9) return "Béo phì độ I - Nguy cơ trung bình";
            if (bmi < 39.9) return "Béo phì độ II - Nguy cơ cao";
            return "Béo phì độ III - Nguy cơ rất cao";
        }

        public async Task<ReportDto> CreateReportAsync(CreateReportDto request)
        {
            var report = new Report
            {
                ReportId = Guid.NewGuid(),
                Height = request.Height,
                Weight = request.Weight,
                BMI = request.Weight / Math.Pow(request.Height / 100, 2), 
                ReprotCreateDate = request.Date,
                ReportIsActive = "2"
            };

            var createdReport = await _reportRepository.CreateReportAsync(report);
            return new ReportDto
            {
                ReportId = createdReport.ReportId,
                ChildId = createdReport.ChildId,
                Height = createdReport.Height,
                Weight = createdReport.Weight,
                BMI = createdReport.BMI,
                ReportCreateDate = createdReport.ReprotCreateDate,
                ReportIsActive = "2"
            };
        }
        public async Task<Report> CreateReportAsync2(Guid childId, CreateReportDto dto)
        {
            return await _reportRepository.CreateReportAsync2(childId, dto);
        }

        public async Task<bool> UpdateReportAsync(Guid reportId, UpdateReportDto request)
        {
            var child = await _childRepository.GetChildByIdAsync(request.ChildId);
            if (child == null)
                throw new KeyNotFoundException("Không tìm thấy thông tin trẻ.");
             
            if (request.Date < child.DOB)
                throw new Exception("Ngày tạo báo cáo không thể nhỏ hơn ngày sinh của trẻ.");

            if (request.Date > DateTime.UtcNow)
                throw new Exception("Ngày tạo báo cáo không thể ở tương lai.");
            
            var reports = await _reportRepository.GetReportsByChildIdAsync(request.ChildId);
            var report = reports.FirstOrDefault(r => r.ReportId == reportId);
            if (report == null) return false;
            
            bool isDuplicateDate = reports.Any(r => r.ReprotCreateDate.Date == request.Date.Date && r.ReportId != reportId);
            if (isDuplicateDate)
                throw new Exception("Đã tồn tại báo cáo vào ngày này.");

            report.Height = request.Height;
            report.Weight = request.Weight;
            report.ReprotCreateDate = request.Date;
            report.BMI = request.Weight / Math.Pow(request.Height / 100, 2);
            report.ReportMark = GetBMICategory(report.BMI);
            
            return await _reportRepository.UpdateReportAsync(report);           
        }
        
        public async Task<bool> DeleteReportByIdAsync(Guid reportId)
        {
            var report = await _reportRepository.GetByIDAsync(reportId);
            if (report == null)
            {
                return false;
            }
            report.ReportIsActive = "3";
            return await _reportRepository.UpdateReportAsync(report);
        }
        public async Task<IEnumerable<ReportDto>> GetReportsByStatusAsync(string status)
        {

            var statusMapping = new Dictionary<string, int>
            {
                { "Active", 1 },
                { "Pending", 0 },
                { "Inactive", 2 },
                { "Delete", 3 }
            };
            if (statusMapping.TryGetValue(status, out int statusInt))
            {
                return await _reportRepository.GetReportsByStatusAsync(statusInt.ToString());
            }

            if (int.TryParse(status, out int statusFromInput))
            {
                return await _reportRepository.GetReportsByStatusAsync(statusFromInput.ToString());
            }
            
            return new List<ReportDto>();
        }

        public async Task<bool> UpdateReportStatusAsync(Guid reportId, string newStatus)
        {
            var report = await _reportRepository.GetAsync(r => r.ReportId == reportId);
            if (report == null) return false;

            report.ReportIsActive = newStatus;
            return await _reportRepository.UpdateReportAsync(report);
        }

        }
    public class ParentDto
    {
        public Guid AccountId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

}