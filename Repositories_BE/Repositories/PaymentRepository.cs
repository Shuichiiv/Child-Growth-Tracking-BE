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

    public async Task<Payment> CreatePaymentAsync(Payment payment)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<Payment> GetPaymentByOrderIdAsync(Guid serviceOrderId)
    {
        return await _context.Payments.FirstOrDefaultAsync(p => p.ServiceOrderId == serviceOrderId);
    }

    public async Task UpdatePaymentAsync(Payment payment)
    {
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();
    }
}