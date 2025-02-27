using DataObjects_BE.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_BE.RatingDTOs
{
    public class RatingResponseDTO
    {
        public Guid RatingId { get; set; }
        public Guid FeedbackId { get; set; }
        public Guid ParentId { get; set; }
        public double RatingValue { get; set; }
        public DateTime RatingDate { get; set; }
        public bool IsActive { get; set; }
        public virtual Feedback Feedback { get; set; }
        public virtual Parent Parent { get; set; }
    }
}
