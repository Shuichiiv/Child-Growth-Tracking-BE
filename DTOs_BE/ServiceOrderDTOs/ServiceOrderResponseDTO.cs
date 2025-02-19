using DataObjects_BE.Entities;
using DTOs_BE.ServiceDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_BE.ServiceOrderDTOs
{
    public class ServiceOrderResponseDTO
    {
        public Guid ServiceOrderId { get; set; }
        public Guid ParentId { get; set; }
        public int ServiceId { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public float TotalPrice { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual Parent Parent { get; set; }
        public virtual ServiceResponseDTO Service { get; set; }
    }
}
