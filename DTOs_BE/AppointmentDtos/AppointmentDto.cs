namespace DTOs_BE.AppointmentDtos
{
    public class AppointmentDto
    {
        public Guid AppointmentId { get; set; }
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; }
        public Guid ParentId { get; set; }
        public string ParentName { get; set; }
        public Guid ChildId { get; set; }
        public string ChildName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
    }

    public class AppointmentCreateDto
    {
        public Guid DoctorId { get; set; }
        public Guid ParentId { get; set; }
        public Guid ChildId { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime AppointmentDate { get; set; } // Ngày hẹn thực tế
        public DateTime CreatedAt { get; set; } // Ngày tạo do người dùng nhập
    }
    
    public class AppointmentUpdateDto
    {
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
    }
}