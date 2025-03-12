using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_BE.ServiceOrderDTOs
{
    public class CreateServiceOrderModel
    {
        public Guid ParentId { get; set; }
        public int ServiceId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
      //  public float UnitPrice { get; set; }
       // public float TotalPrice { get; set; }
    }
}
