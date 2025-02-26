using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_BE.FeedbackDTOs
{
    public class UpdateFeedbackModel
    {
        public string FeedbackContentRequest { get; set; }
        public string FeedbackName { get; set; }
        public string FeedbackContentResponse { get; set; }
    }
}
