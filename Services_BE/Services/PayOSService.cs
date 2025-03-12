using System.Security.Cryptography;
using System.Text;
using DataObjects_BE;
using DataObjects_BE.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace Services_BE.Services
{

    public class PayOSService
    {
        private readonly string _clientId;
        private readonly string _apiKey;
        private readonly string _checksumKey;
        private readonly SWP391G3DbContext _dbContext;
        private readonly string _baseUrl = "https://api.payos.vn/v2/payment-requests";

        public PayOSService(IConfiguration configuration)
        {
            _clientId = configuration["PayOS:ClientId"];
            _apiKey = configuration["PayOS:ApiKey"];
            _checksumKey = configuration["PayOS:ChecksumKey"];
        }
        public async Task<bool> CreatePaymentAsync(Guid parentId, int serviceId, string paymentMethod)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // 1. Lấy thông tin dịch vụ
                var service = await _dbContext.Services.FindAsync(serviceId);
                if (service == null) throw new Exception("Dịch vụ không tồn tại!");

                // 2. Tạo đơn hàng (ServiceOrder)
                var serviceOrder = new ServiceOrder
                {
                    ServiceOrderId = Guid.NewGuid(),
                    ParentId = parentId,
                    ServiceId = serviceId,
                    Quantity = 1,
                    UnitPrice = (float)service.ServicePrice,
                    TotalPrice = (float)service.ServicePrice,
                    CreateDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(service.ServiceDuration) // Tính hạn dùng
                };

                _dbContext.ServiceOrders.Add(serviceOrder);
                await _dbContext.SaveChangesAsync();

                // 3. Tạo giao dịch thanh toán
                var payment = new Payment
                {
                    PaymentId = Guid.NewGuid(),
                    ServiceOrderId = serviceOrder.ServiceOrderId,
                    PaymentMethod = paymentMethod,
                    PaymentStatus = PaymentStatus.Pending, // Chờ xác nhận thanh toán
                    Amount = service.ServicePrice,
                    PaymentDate = DateTime.UtcNow
                };

                _dbContext.Payments.Add(payment);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Lỗi thanh toán: {ex.Message}");
            }
        }



        private string GenerateChecksum(string data)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_checksumKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}