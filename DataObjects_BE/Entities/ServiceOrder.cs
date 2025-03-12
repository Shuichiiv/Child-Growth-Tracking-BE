using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects_BE.Entities
{
    public partial class ServiceOrder
    {
        [Key]
        public Guid ServiceOrderId {  get; set; }
        public Guid ParentId { get; set; }
        public int ServiceId { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public float TotalPrice { get; set; }
        public DateTime  CreateDate { get; set; } = DateTime.UtcNow;
        
        public string Status { get; set; } = "Pending";
        
        public long? OrderCode { get; set; }
        
        public DateTime EndDate { get; set; }
        [ForeignKey("ParentId")]
        public virtual Parent Parent { get; set; }
        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }
        
        public void CalculateEndDate()
        {
            if (Service != null)
            {
                EndDate = CreateDate.AddDays(Service.ServiceDuration);
            }
        }
    }
}
