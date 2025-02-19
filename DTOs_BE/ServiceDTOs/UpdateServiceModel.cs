using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_BE.ServiceDTOs
{
    public class UpdateServiceModel
    {
        public string ServiceName { get; set; }
        public decimal ServicePrice { get; set; }
        public string ServiceDescription { get; set; }
        public float ServiceDuration { get; set; }
        public bool IsActive { get; set; }
    }
}
