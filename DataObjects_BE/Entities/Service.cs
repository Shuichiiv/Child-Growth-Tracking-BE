using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects_BE.Entities
{
    public partial class Service
    {
        [Key]
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } 
        public decimal ServicePrice { get; set; }
        public string ServiceDescription { get; set; }
        public float ServiceDuration { get; set; }
        public DateTime ServiceCreateDate { get; set; }
        public bool IsActive {  get; set; }
        public virtual ICollection<ServiceOrder> ServiceOrders { get; set; }
    }
}
