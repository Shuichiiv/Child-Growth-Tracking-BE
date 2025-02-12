using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects_BE.Entities
{
    public partial class Rating
    {
        public Guid RatingId { get; set; }
        public Guid FeedbackId { get; set; }
        public Guid ParentId { get; set; }
        public double RatingValue { get; set; }
        public DateTime RatingDate { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("FeedbackId")]
        public virtual Feedback Feedback { get; set; }
        [ForeignKey("ParentId")]
        public virtual Parent Parent { get; set; }
       
    }
}
