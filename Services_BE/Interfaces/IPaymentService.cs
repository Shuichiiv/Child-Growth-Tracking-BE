namespace Services_BE.Interfaces;

public interface IPaymentService
{
    Task<string> CreatePaymentAsync(Guid parentId, int serviceId, int quantity, string paymentMethod);
    Task<bool> HandlePaymentCallbackAsync(Guid paymentId, bool success, string transactionId);
    
}