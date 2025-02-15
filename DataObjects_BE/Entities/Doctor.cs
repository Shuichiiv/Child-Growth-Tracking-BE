using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects_BE.Entities
{
    public partial class Doctor
    {
        [Key]
        public Guid DoctorId { get; set; }
        public Guid AccountId { get; set; }
        public string Specialization {  get; set; }
        public int ExperienceYears { get; set; }
        public string HospitalAddressWork {  get; set; }

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}