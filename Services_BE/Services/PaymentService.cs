using DataObjects_BE.Entities;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using DataObjects_BE;
using DTOs_BE.UserDTOs;
using Repositories_BE.Interfaces;
using Services_BE.Interfaces;

public class PaymentService : IPaymentService
{
    private readonly SWP391G3DbContext _context;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string PayOS_ClientID = "9248e8d9-9110-4c4d-96e1-f61cec695c91"; // Thay thế bằng Client ID của bạn
    private readonly string PayOS_API_URL = "https://payos.vn/api/v1"; // URL của PayOS

    public PaymentService(SWP391G3DbContext context, IPaymentRepository paymentRepository, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _paymentRepository = paymentRepository;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> CreatePaymentAsync(Guid parentId, int serviceId, int quantity, string paymentMethod)
    {
        var service = await _context.Services.FindAsync(serviceId);
        if (service == null) throw new Exception("Dịch vụ không tồn tại.");

        // Tạo đơn hàng
        var serviceOrder = new ServiceOrder
        {
            ServiceOrderId = Guid.NewGuid(),
            ParentId = parentId,
            ServiceId = serviceId,
            Quantity = quantity,
            UnitPrice = (float)service.ServicePrice,
            TotalPrice = (float)(service.ServicePrice * quantity),
            CreateDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddMonths(1)
        };
        await _context.ServiceOrders.AddAsync(serviceOrder);
        await _context.SaveChangesAsync();

        // Tạo thanh toán
        var payment = new Payment
        {
            PaymentId = Guid.NewGuid(),
            ServiceOrderId = serviceOrder.ServiceOrderId,
            PaymentMethod = paymentMethod,
            Amount = (decimal)serviceOrder.TotalPrice,
            PaymentStatus = PaymentStatus.Pending
        };

        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();

        // Gửi yêu cầu đến PayOS
        var httpClient = _httpClientFactory.CreateClient();
        var requestBody = new
        {
            client_id = PayOS_ClientID,
            order_code = payment.PaymentId.ToString(),
            amount = payment.Amount,
            description = $"Thanh toán dịch vụ {service.ServiceName}",
            return_url = "https://localhost:7190/payment-success",
            cancel_url = "https://localhost:7190/payment-fail"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync($"{PayOS_API_URL}/create", jsonContent);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonSerializer.Deserialize<PayOSResponse>(responseBody);
            payment.PaymentUrl = jsonResponse.payment_url;
            await _context.SaveChangesAsync();
            return jsonResponse.payment_url;
        }

        return null;
    }

    public async Task<bool> HandlePaymentCallbackAsync(Guid paymentId, bool success, string transactionId)
    {
        var status = success ? PaymentStatus.Completed : PaymentStatus.Failed;
        return await _paymentRepository.UpdatePaymentStatusAsync(paymentId, status, transactionId);
    }
}
