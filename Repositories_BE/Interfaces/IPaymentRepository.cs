using DataObjects_BE.Entities;

namespace Repositories_BE.Interfaces
{
    public interface IPaymentRepository
    {
        Task AddPaymentAsync(Payment payment);
        Task AddPayment(Payment payment);
        Task<List<Payment>> GetPaymentsByOrderId(Guid orderId);
        Task DeletePayment(Payment payment);
    }
}