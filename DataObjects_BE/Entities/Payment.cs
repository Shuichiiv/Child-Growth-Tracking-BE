using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects_BE.Entities
{
    public partial class Payment
    {
        [Key]
        public Guid PaymentId { get; set; }
        public Guid ServiceOrderId { get; set; }
        public string PaymentMethod {  get; set; }
        public PaymentStatus PaymentStatus {  get; set; } = PaymentStatus.Pending;
        public DateTime PaymentDate { get; set; } = default;
        public decimal Amount { get; set; }
        [ForeignKey("ServiceOrderId")]
        public virtual ServiceOrder ServiceOrder { get; set; }

    }
    public enum PaymentStatus
    {
        Pending = 0,
        Completed = 1,
        Failed = 2
    }
}
