using AutoMapper;
using DataObjects_BE;
using DataObjects_BE.Entities;
using DTOs_BE.UserDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Repositories_BE.Interfaces;

namespace Services_BE.Services;

public class ServiceOrderServiceP
{
    private readonly IServiceOrderRepository _serviceOrderRepository;
    private readonly IServiceRepositoy _serviceRepositoy;
    private readonly HttpClient _httpClient;
    private readonly string _clientId = "9248e8d9-9110-4c4d-96e1-f61cec695c91";
    private readonly string _apiKey = "6fda83ce-45f3-4071-a327-7ed27f5e3fca";

    public ServiceOrderServiceP(IServiceOrderRepository serviceOrderRepository,HttpClient httpClient, IServiceRepositoy serviceRepositoy)
    {
        _serviceOrderRepository = serviceOrderRepository;
        _httpClient = httpClient;
        _serviceRepositoy = serviceRepositoy;
    }
    
    /// <summary>
    /// Truy xuất ServiceOrder dựa vào ParentId
    /// </summary>
    public async Task<ServiceOrder?> GetServiceOrderByParentId(Guid parentId)
    {
        return await _serviceOrderRepository.GetLatestServiceOrderByParentIdAsync(parentId);
    }
    
    /// <summary>
    /// Gọi API PayOS để kiểm tra trạng thái thanh toán
    /// </summary>
    public async Task<string?> GetPaymentStatusFromPayOS(long orderCode)
    {
        string apiUrl = $"https://api-merchant.payos.vn/v2/payment-requests/{orderCode}";

        _httpClient.DefaultRequestHeaders.Add("x-client-id", _clientId);
        _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);

        HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<PayOSResponse>(responseBody);
            return jsonResponse?.Data?.Status;
        }

        return null;
    }
    /// <summary>
    /// Cập nhật trạng thái ServiceOrder nếu thanh toán thành công
    /// </summary>
    /*public async Task UpdateServiceOrderStatus(Guid parentId)
    {
        var serviceOrder = await _context.ServiceOrders
            .FirstOrDefaultAsync(so => so.ParentId == parentId);

        if (serviceOrder == null || serviceOrder.OrderCode == null)
        {
            Console.WriteLine("Order not found or OrderCode is null.");
            return;
        }
        
        string? status = await GetPaymentStatusFromPayOS(serviceOrder.OrderCode.Value);
        
        if (status?.ToUpper() == "CANCELLED")
        {
            serviceOrder.Status = "Cancelled";

            if (_context.Entry(serviceOrder).State == EntityState.Detached)
            {
                _context.ServiceOrders.Attach(serviceOrder);
            }
            _context.Entry(serviceOrder).State = EntityState.Modified;
            
            var existingPayment = await _context.Payments
                .FirstOrDefaultAsync(p => p.ServiceOrderId == serviceOrder.ServiceOrderId);

            if (existingPayment != null)
            {
                existingPayment.PaymentStatus = PaymentStatus.Cancelled;
                existingPayment.PaymentDate = DateTime.UtcNow;
                _context.Entry(existingPayment).State = EntityState.Modified;
            }
            else
            {
                Console.WriteLine("Payment record not found.");
            }
            var rowsAffected = await _context.SaveChangesAsync();
            Console.WriteLine($"Rows Affected: {rowsAffected}");
        }
        
        else if (status == "PAID")
        {
            serviceOrder.Status = "Completed";

            if (_context.Entry(serviceOrder).State == EntityState.Detached)
            {
                _context.ServiceOrders.Attach(serviceOrder);
            }
            _context.Entry(serviceOrder).State = EntityState.Modified;
            
            var existingPayment = await _context.Payments
                .FirstOrDefaultAsync(p => p.ServiceOrderId == serviceOrder.ServiceOrderId);

            if (existingPayment != null)
            {
                existingPayment.PaymentStatus = PaymentStatus.Completed;
                existingPayment.PaymentDate = DateTime.UtcNow;
                _context.Entry(existingPayment).State = EntityState.Modified;
            }
            else
            {
                Console.WriteLine("Payment record not found.");
            }
            /*var payment = new Payment
            {
                PaymentId = Guid.NewGuid(),
                ServiceOrderId = serviceOrder.ServiceOrderId,
                PaymentMethod = "PayOS",
                PaymentStatus = PaymentStatus.Completed,
                PaymentDate = DateTime.UtcNow,
                Amount = (decimal)serviceOrder.TotalPrice,
                TransactionId = serviceOrder.OrderCode.ToString()
            };

            _context.Payments.Add(payment);#1#
            var rowsAffected = await _context.SaveChangesAsync();
            Console.WriteLine($"Rows Affected: {rowsAffected}");
        }
        
    }*/
    public async Task UpdateServiceOrderStatus(Guid parentId)
    {
        /*var serviceOrders = await _context.ServiceOrders
            .Where(so => so.ParentId == parentId)
            .OrderByDescending(so => so.CreateDate) // Lấy đơn mới nhất trước
            .ToListAsync();*/
        var serviceOrders = await _serviceOrderRepository.GetServiceOrdersByParentIdAsync(parentId);

        if (!serviceOrders.Any())
        {
            Console.WriteLine("No service orders found.");
            return;
        }

        foreach (var serviceOrder in serviceOrders)
        {
            if (serviceOrder.OrderCode == null)
            {
                Console.WriteLine($"OrderCode is null for ServiceOrderId: {serviceOrder.ServiceOrderId}");
                continue;
            }

            // Gọi API để lấy trạng thái mới nhất
            string? status = (await GetPaymentStatusFromPayOS(serviceOrder.OrderCode.Value))?.ToUpper();

            
            if (string.IsNullOrEmpty(status)) continue; // Không cập nhật nếu trạng thái không hợp lệ

            if (status == "CANCELLED")
            {
                serviceOrder.Status = "Cancelled";
            }
            else if (status == "PAID")
            {
                serviceOrder.Status = "Completed";
            }
        }

        await _serviceOrderRepository.SaveChangesAsync();
        Console.WriteLine("Service order statuses updated.");
    }

    public async Task<ServiceOrder> CreateServiceOrderAsync(Guid parentId, int serviceId, int quantity)
    {
        var service = await _serviceRepositoy.GetServiceByIdAsync(serviceId);
        if (service == null) throw new Exception("Service not found");

        var serviceOrder = new ServiceOrder
        {
            ServiceOrderId = Guid.NewGuid(),
            ParentId = parentId,
            ServiceId = serviceId,
            Quantity = quantity,
            UnitPrice = (float)service.ServicePrice,
            TotalPrice = (float)(service.ServicePrice * quantity),
            CreateDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddMonths(1),
            OrderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };
        serviceOrder.CalculateEndDate();

        await _serviceOrderRepository.AddServiceOrderAsync(serviceOrder);
        await _serviceOrderRepository.SaveChangesAsync();
        return serviceOrder;
    }

    /// <summary>
    /// Model JSON trả về từ API PayOS
    /// </summary>
    public class PayOSResponse
    {
        public string Code { get; set; }
        public string Desc { get; set; }
        public PayOSData Data { get; set; }
    }

    public class PayOSData
    {
        public long OrderCode { get; set; }
        public string Status { get; set; }
    }

}