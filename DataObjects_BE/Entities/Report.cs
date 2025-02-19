using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects_BE.Entities
{
    public partial class Report
    {
        [Key]
        public Guid ReportId { get; set; }
        public Guid ChildId { get; set; }
        public string ReportMark { get; set; }
        public string ReportContent { get; set; }
        public DateTime ReprotCreateDate { get; set; }
        public string ReportIsActive { get; set; } // Active, Pending, Inactive
        public string ReportName { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double BMI { get; set; }
        [ForeignKey("ChildId")]
        public virtual Child Child { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        //public virtual ProductList ProductList { get; set; }
        public virtual ICollection<ReportProduct> ReportProducts { get; set; }
    }
}
