using DataObjects_BE;
using DataObjects_BE.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories_BE.Interfaces;

namespace Repositories_BE.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly SWP391G3DbContext _context;

    public PaymentRepository(SWP391G3DbContext context)
    {
        _context = context;
    }
    public async Task AddPaymentAsync(Payment payment)
    {
        _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
    }
    public async Task AddPayment(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
    }

    /*
    public async Task<Payment> GetByIdAsync(Guid paymentId)
    {
        return await _context.Payments.FindAsync(paymentId);
    }

    public async Task UpdateAsync(Payment payment)
    {
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();
    }
    public async Task<bool> UpdatePaymentStatusAsync(Guid paymentId, PaymentStatus status, string transactionId)
    {
        var payment = await _context.Payments.FindAsync(paymentId);
        if (payment == null)
        {
            return false;
        }

        payment.PaymentStatus = status;
        payment.TransactionId = transactionId;
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();
    
        return true;
    }
    public async Task<Payment> CreatePaymentAsync(Guid serviceOrderId, decimal amount, string method)
    {
        var payment = new Payment
        {
            ServiceOrderId = serviceOrderId,
            Amount = amount,
            PaymentMethod = method,
            PaymentStatus = 0, // Pending
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<bool> UpdatePaymentStatusAsync(Guid paymentId, int status, string transactionId)
    {
        var payment = await _context.Payments.FindAsync(paymentId);
        if (payment == null) return false;

        payment.PaymentStatus = (PaymentStatus)status;
        payment.TransactionId = transactionId;
        await _context.SaveChangesAsync();
        return true;
    }*/
    public async Task<List<Payment>> GetPaymentsByOrderId(Guid orderId)
    {
        var list = await _context.Payments.Where(x => x.ServiceOrderId == orderId).ToListAsync();
        return list;
    }
    public async Task DeletePayment(Payment payment)
    {
         _context.Remove(payment);
    }

}