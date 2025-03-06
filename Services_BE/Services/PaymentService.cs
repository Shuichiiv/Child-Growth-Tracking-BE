using DataObjects_BE.Entities;
using Repositories_BE.Interfaces;

namespace Services_BE.Services;

public class PaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<Payment> GenerateVietQRAsync(Guid serviceOrderId, decimal amount)
    {
        var payment = new Payment
        {
            PaymentId = Guid.NewGuid(),
            ServiceOrderId = serviceOrderId,
            PaymentMethod = "VietQR",
            PaymentStatus = PaymentStatus.Pending,
            Amount = amount
        };

        return await _paymentRepository.CreatePaymentAsync(payment);
    }

    public async Task<bool> ConfirmPaymentAsync(Guid serviceOrderId, bool isPaid)
    {
        var payment = await _paymentRepository.GetPaymentByOrderIdAsync(serviceOrderId);
        if (payment == null) return false;

        payment.PaymentStatus = isPaid ? PaymentStatus.Completed : PaymentStatus.Failed;
        payment.PaymentDate = DateTime.UtcNow;
        await _paymentRepository.UpdatePaymentAsync(payment);
        return true;
    }

    public async Task<PaymentStatus> CheckPaymentStatusAsync(Guid serviceOrderId)
    {
        var payment = await _paymentRepository.GetPaymentByOrderIdAsync(serviceOrderId);
        return payment?.PaymentStatus ?? PaymentStatus.Pending;
    }
}