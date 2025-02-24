using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataObjects_BE.Entities
{
    public class Appointment
    {
        [Key] 
        public Guid AppointmentId { get; set; }

        [Required] 
        public Guid DoctorId { get; set; }

        [Required] 
        public Guid ParentId { get; set; }

        public Guid? ChildId { get; set; }

        [Required] 
        public DateTime ScheduledTime { get; set; }

        [Required] 
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        [ForeignKey("DoctorId")] 
        public virtual Doctor Doctor { get; set; }

        [ForeignKey("ParentId")] 
        public virtual Parent Parent { get; set; }

        [ForeignKey("ChildId")] 
        public virtual Child Child { get; set; }
    }
    public enum AppointmentStatus
    {
        Pending,    // Đang chờ xác nhận
        Confirmed,  // Đã xác nhận
        Canceled    // Đã hủy
    }
}