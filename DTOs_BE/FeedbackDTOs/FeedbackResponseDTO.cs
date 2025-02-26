using DataObjects_BE.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_BE.FeedbackDTOs
{
    public class FeedbackResponseDTO
    {
        public Guid FeedbackId { get; set; }
        public Guid ReportId { get; set; }
        public Guid DoctorId { get; set; }
        public string FeedbackContentRequest { get; set; }
        public DateTime FeedbackCreateDate { get; set; }
        public DateTime FeedbackUpdateDate { get; set; }
        public bool FeedbackIsActive { get; set; }
        public string FeedbackName { get; set; }
        public string FeedbackContentResponse { get; set; }
        public virtual Report Report { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
