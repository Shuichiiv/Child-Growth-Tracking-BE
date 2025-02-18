namespace DTOs_BE.DoctorDTOs
{
    public class ReportDto
    {
        public Guid ReportId { get; set; }
        public Guid ChildId { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double BMI { get; set; }
        public string ReportMark { get; set; } // Phân loại BMI
        public string ReportContent { get; set; } // Nội dung chi tiết kết quả BMI
        public DateTime ReportCreateDate { get; set; }
    }
}

