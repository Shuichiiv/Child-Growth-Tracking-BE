using DataObjects_BE.Entities;

namespace Repositories_BE.Interfaces
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
        Task<Payment> GetByIdAsync(Guid paymentId);
        Task UpdateAsync(Payment payment);
        Task<bool> UpdatePaymentStatusAsync(Guid paymentId, PaymentStatus status, string transactionId);
        
        Task<Payment> CreatePaymentAsync(Guid serviceOrderId, decimal amount, string method);
        Task<bool> UpdatePaymentStatusAsync(Guid paymentId, int status, string transactionId);
        Task AddPayment(Payment payment);
        Task<List<Payment>> GetPaymentsByOrderId(Guid orderId);
        Task DeletePayment(Payment payment);
    }
}