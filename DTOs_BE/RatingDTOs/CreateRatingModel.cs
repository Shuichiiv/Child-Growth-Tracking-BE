using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_BE.RatingDTOs
{
    public class CreateRatingModel
    {
        public Guid FeedbackId { get; set; }
        public Guid ParentId { get; set; }
        public double RatingValue { get; set; }
        public DateTime RatingDate { get; set; }
        public bool IsActive { get; set; }
    }
}
