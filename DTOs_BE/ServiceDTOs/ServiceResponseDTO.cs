using DataObjects_BE.Entities;
using DTOs_BE.ServiceOrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_BE.ServiceDTOs
{
    public class ServiceResponseDTO
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public float ServicePrice { get; set; }
        public string ServiceDescription { get; set; }
        public float ServiceDuration { get; set; }
        public DateTime ServiceCreateDate { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<ServiceOrderResponseDTO> ServiceOrders { get; set; }
    }
}
