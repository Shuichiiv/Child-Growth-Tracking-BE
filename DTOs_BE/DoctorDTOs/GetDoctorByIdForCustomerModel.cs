using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_BE.DoctorDTOs
{
    public class GetDoctorByIdForCustomerModel
    {
        public Guid DoctorId { get; set; }
        public Guid AccountId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}
