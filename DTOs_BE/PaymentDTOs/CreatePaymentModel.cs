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
    
    public class Item
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }

        public Item(string name, int quantity, long price)
        {
            Name = name;
            Quantity = quantity;
            Price = price;
        }
    }
    public class CreateServiceOrderRequest
    {
        public Guid ParentId { get; set; }
        public int ServiceId { get; set; }
        public int Quantity { get; set; }
    }

    public class CreatePaymentRequest
    {
        public Guid ServiceOrderId { get; set; }
        public string PaymentMethod { get; set; }
    }

    public class PaymentRequestModel
    {
        public Guid ParentId { get; set; } 
        public string Description { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
        public List<ServiceOrderModel> Services { get; set; }
    }

    public class ServiceOrderModel
    {
        public int ServiceId { get; set; }
        public int Quantity { get; set; }
        public float TotalPrice { get; set; }
    }
}
