using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_BE.ServiceOrderDTOs
{
    public class CheckServiceRightsModel
    {
        public int? ServiceId { get; set; }
        /*public string Status { get; set; }*/
        public bool IsValid { get; set; }
    }
}
