using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects_BE.Entities
{
    public partial class Child
    {
        public Guid ChildId { get; set; }
        public Guid ParentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender {  get; set; }
        public DateTime DOB {  get; set; }
        public DateTime DateCreateAt { get; set; }
        public DateTime DateUpdateAt { get; set;}
        public string ImageUrl { get; set; }
        [ForeignKey("ParentId")]
        public virtual Parent Parent {  get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
