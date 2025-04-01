using DataObjects_BE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using DataObjects_BE;
using DTOs_BE.UserDTOs;
using Repositories_BE.Interfaces;
using Services_BE.Interfaces;
using DTOs_BE.PaymentDTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Net.payOS;
using Net.payOS.Types;
using Services_BE.Services;

public class PaymentService : IPaymentService
{
    private readonly ServiceOrderServiceP _serviceOrderService;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IServiceOrderRepository _serviceOrderRepository;
    private readonly IConfiguration _config;
    private readonly ILogger<PaymentService> _logger;
    private readonly PayOS _payOS;

    public PaymentService(ServiceOrderServiceP serviceOrderService, IPaymentRepository paymentRepository,
        IConfiguration config, ILogger<PaymentService> logger, SWP391G3DbContext context, IServiceOrderRepository serviceOrderRepository)
    {
        _serviceOrderRepository = serviceOrderRepository;
        _serviceOrderService = serviceOrderService;
        _paymentRepository = paymentRepository;
        _config = config;
        _logger = logger;
        
        var clientId = _config["PayOS:ClientId"];
        var apiKey = _config["PayOS:ApiKey"];
        var checksumKey = _config["PayOS:ChecksumKey"];
        _payOS = new PayOS(clientId, apiKey, checksumKey);
    }
     public async Task<(bool Success, string Message, string PaymentUrl)> CreatePaymentAsync(PaymentRequestModel request)
    {
        if (request == null || request.Services == null || !request.Services.Any())
        {
            return (false, "Invalid request data", null);
        }
        
        
        var createdServiceOrders = new List<ServiceOrder>();
        foreach (var service in request.Services)
        {
            // Lấy đơn hàng mới nhất cho dịch vụ này
            var existingOrder = await _serviceOrderRepository
                .GetLatestServiceOrderByParentIdAndServiceId(request.ParentId, service.ServiceId);

            if (existingOrder != null)
            {
                // Nếu đơn hàng có trạng thái Cancelled, tạo đơn hàng mới
                if (existingOrder.Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
                {
                    var newOrder = await _serviceOrderService
                        .CreateServiceOrderAsync(request.ParentId, service.ServiceId, service.Quantity);
                    createdServiceOrders.Add(newOrder);
                }
                else
                {
                    // Nếu đơn hàng còn hiệu lực (Pending hoặc Active), dùng lại đơn đó
                    createdServiceOrders.Add(existingOrder);
                }
            }
            else
            {
                // Nếu chưa có đơn hàng nào, tạo mới
                var newOrder = await _serviceOrderService
                    .CreateServiceOrderAsync(request.ParentId, service.ServiceId, service.Quantity);
                createdServiceOrders.Add(newOrder);
            }
        }

        /*var serviceOrders = await _serviceOrderRepository
            .GetServiceOrdersByParentIdAndServiceIds(request.ParentId, request.Services.Select(s => s.ServiceId).ToList());*/
        var serviceOrders = (await _serviceOrderRepository
                .GetServiceOrdersByParentIdAndServiceIds(request.ParentId, request.Services.Select(s => s.ServiceId).ToList()))
            .Where(so => !so.Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
            .ToList();

        var latestServiceOrders = await _serviceOrderRepository
            .GetLatestServiceOrdersByParentId(request.ParentId);
        //tinh tong tien don hang
        decimal totalAmount1 = serviceOrders.Sum(so => (decimal)so.TotalPrice);
        
        var serviceList = serviceOrders.Select(so => new ItemData(
            name: so.Service.ServiceName,
            quantity: 1,
            price: (int)so.TotalPrice
        )).ToList();
        
        //Lay thong tin tu appsetting
        var clientId = _config["PayOS:ClientId"];
        var apiKey = _config["PayOS:ApiKey"];
        var checksumKey = _config["PayOS:ChecksumKey"];
        
        //Lay orderCode tu db
        long orderCode = (long)serviceOrders.First().OrderCode;
        
        //Tao data goi len cho payOS
        var paymentRequest = new PaymentData(
            orderCode: orderCode,
            amount: (int)totalAmount1,
            description: request.Description,
            items: serviceList,
            returnUrl: request.ReturnUrl,
            cancelUrl: request.CancelUrl
        );
        try
        {
            var response = await _payOS.createPaymentLink(paymentRequest);
            if (response == null || string.IsNullOrEmpty(response.checkoutUrl))
            {
                return (false, "Failed to get checkout URL from PayOS", null);
            }

            var payment = new Payment
            {
                PaymentId = Guid.NewGuid(),
                Amount = totalAmount1,
                PaymentDate = DateTime.UtcNow,
                PaymentMethod = "PayOS",
                PaymentStatus = PaymentStatus.Pending,
                PaymentUrl = response.checkoutUrl,
                ServiceOrderId = latestServiceOrders.First().ServiceOrderId,
                TransactionId = orderCode.ToString()
            };

            await _paymentRepository.AddPaymentAsync(payment);
            return (true, "Payment created successfully", response.checkoutUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating payment");
            return (false, "Failed to create payment", null);
        }
    }
     
    public async Task CreateCashPayment( ServiceOrder order, PaymentStatus paymentStatus)
    {
        try
        {
            Payment newPayment = new Payment
            {
                PaymentId = Guid.NewGuid(),
                ServiceOrderId = order.ServiceOrderId,
                PaymentMethod = "Cash",
                PaymentStatus = paymentStatus,
                Amount = (decimal)order.TotalPrice,
            };
            await _paymentRepository.AddPayment(newPayment);
        }catch(Exception ex)
        {
            throw ex;
        }
    }
    
}
