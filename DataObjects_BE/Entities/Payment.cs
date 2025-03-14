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
        public string PaymentMethod { get; set; }
        
        [Column(TypeName = "nvarchar(20)")]
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public decimal Amount { get; set; }

        [ForeignKey("ServiceOrderId")]
        public virtual ServiceOrder ServiceOrder { get; set; }

        public string? PaymentUrl { get; set; }
        public string? TransactionId { get; set; }
        public string? Signature { get; set; }
    }

    public enum PaymentStatus
    {
        Pending = 0,
        Completed = 1,
        Failed = 2,
        Cancelled = 3
    }
}
