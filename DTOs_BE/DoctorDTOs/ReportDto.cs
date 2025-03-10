namespace DTOs_BE.DoctorDTOs
{
    public class ReportDto
    {
        public Guid ParentId { get; set; }
        public Guid ReportId { get; set; }
        public Guid ChildId { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double BMI { get; set; }
        public string ReportMark { get; set; } // Phân loại BMI
        public string ReportContent { get; set; } // Nội dung chi tiết kết quả BMI
        public DateTime ReportCreateDate { get; set; }
        
        public string? ReportIsActive { get; set; } // Active, Pending, Inactive

    }
    
    public class ReportDtoFParents
    {
        public Guid ChildId { get; set; }
        public string ReportMark { get; set; }
        public string ReportContent { get; set; }
        public DateTime ReportCreateDate { get; set; }
        public string ReportIsActive { get; set; }
        public string ReportName { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double BMI { get; set; }
    }
    
    public class CreateReportDto
    {
        public double Height { get; set; }
        public double Weight { get; set; }
        public DateTime Date { get; set; }
    }

    public class UpdateReportDto
    {
        public Guid ChildId { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public DateTime Date { get; set; }
    }

    public class ReportDto2
    {
        public Guid ReportId { get; set; }
        public Guid ChildId { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double BMI { get; set; }
        public DateTime CreateDate { get; set; }
    }

}

