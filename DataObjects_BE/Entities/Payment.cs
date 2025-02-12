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
        public int PaymentStatus {  get; set; }
        public DateTime PaymentDate { get; set; } = default;
        public string Amount { get; set; }
        [ForeignKey("ServiceOrderId")]
        public virtual ServiceOrder ServiceOrder { get; set; }

    }
}
