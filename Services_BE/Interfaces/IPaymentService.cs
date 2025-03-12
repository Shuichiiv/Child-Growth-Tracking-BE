using DataObjects_BE.Entities;
using DTOs_BE.PaymentDTOs;

namespace Services_BE.Interfaces;

public interface IPaymentService
{
    Task<string> CreatePaymentAsync(Guid parentId, int serviceId, int quantity, string paymentMethod);
    Task<bool> HandlePaymentCallbackAsync(Guid paymentId, bool success, string transactionId);
    Task CreateCashPayment(ServiceOrder order, PaymentStatus paymentStatus);


}