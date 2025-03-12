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
        public DateTime ScheduledTime { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    
    public class AppointmentUpdateDto
    {
        public DateTime ScheduledTime { get; set; }
        public int Status { get; set; }
    }
}