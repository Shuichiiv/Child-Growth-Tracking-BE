using DataObjects_BE.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_BE.PaymentDTOs
{
    public class CreatePaymentModel
    {
        public Guid PaymentId { get; set; }
        public Guid ServiceOrderId { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
    }
}
