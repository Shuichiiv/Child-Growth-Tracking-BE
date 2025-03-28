using DataObjects_BE.Entities;
using DTOs_BE.PaymentDTOs;

namespace Services_BE.Interfaces;

public interface IPaymentService
{
    Task<(bool Success, string Message, string PaymentUrl)> CreatePaymentAsync(PaymentRequestModel request);
    Task CreateCashPayment(ServiceOrder order, PaymentStatus paymentStatus);


}