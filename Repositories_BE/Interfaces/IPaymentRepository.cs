using DataObjects_BE.Entities;

namespace Repositories_BE.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> CreatePaymentAsync(Payment payment);
        Task<Payment> GetPaymentByOrderIdAsync(Guid serviceOrderId);
        Task UpdatePaymentAsync(Payment payment);
    }
}