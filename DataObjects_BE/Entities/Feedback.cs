using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects_BE.Entities
{
    public partial class Feedback
    {
        public Guid FeedbackId {  get; set; }
        public Guid ReportId { get; set; }
        public Guid DoctorId { get; set; }
        public string FeedbackContentRequest { get; set; }
        public DateTime FeedbackCreateDate { get; set; }
        public DateTime FeedbackUpdateDate { get;set; }
        public bool FeedbackIsActive { get; set; } 
        public string FeedbackName { get; set; }
        public string FeedbackContentResponse { get; set; }
        [ForeignKey("ReportId")]
        public virtual Report Report { get; set; }
        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
