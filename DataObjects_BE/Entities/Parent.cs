using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects_BE.Entities
{
    public partial class Parent
    {
        public Guid ParentId { get; set; }
        public Guid AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
        public virtual ICollection<ServiceOrder> ServiceOrders { get; set; }
        public virtual ICollection<Child> Childs { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
