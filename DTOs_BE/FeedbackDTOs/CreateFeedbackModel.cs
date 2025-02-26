using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_BE.FeedbackDTOs
{
    public class CreateFeedbackModel
    {
        public Guid ReportId { get; set; }
        public Guid DoctorId { get; set; }
        public string FeedbackContentRequest { get; set; }
        public bool FeedbackIsActive { get; set; }
        public string FeedbackName { get; set; }
        public string FeedbackContentResponse { get; set; }
    }
}
