using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataObjects_BE;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DataObjects_BE.Entities;
using Microsoft.EntityFrameworkCore;
using Net.payOS;
using Net.payOS.Types;
using Repositories_BE.Interfaces;
using Services_BE.Services;


namespace PaymentIntegration.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly ServiceOrderServiceP _serviceOrderService;
        private readonly PaymentServicesP _paymentService;
        private readonly PayOS _payOS;
        private readonly ILogger<PaymentController> _logger;
        private readonly IConfiguration _config;
        private readonly SWP391G3DbContext _context;
        //private readonly string _checksumKey = "9b109c4b0cb43ef7ecef7ded712d21e056062355891228932016380fd4c947a7";

        public PaymentController(ServiceOrderServiceP serviceOrderService, PaymentServicesP paymentService,
            IConfiguration config, SWP391G3DbContext context, ILogger<PaymentController> logger)
        {
            _serviceOrderService = serviceOrderService;
            _paymentService = paymentService;
            _context = context;
            _logger = logger;
            _config = config;
            var clientId = _config["PayOS:ClientId"];
            var apiKey = _config["PayOS:ApiKey"];
            var checksumKey = _config["PayOS:ChecksumKey"];

            _payOS = new PayOS(clientId, apiKey, checksumKey);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequestModel request)
        {
            // Kiểm tra dữ liệu đầu vào
            if (request == null || request.Services == null || !request.Services.Any())
            {
                return BadRequest(new { message = "Invalid request data" });
            }

            // Tạo `ServiceOrder` trước khi tiếp tục
            var createdServiceOrders = new List<ServiceOrder>();
            foreach (var service in request.Services)
            {
                var serviceOrder =
                    await _serviceOrderService.CreateServiceOrderAsync(request.ParentId, service.ServiceId,
                        service.Quantity);
                createdServiceOrders.Add(serviceOrder);
            }

            //  Truy vấn danh sách ServiceOrders từ DB
            var serviceOrders = await _context.ServiceOrders
                .Include(so => so.Service)
                .Where(so =>
                    so.ParentId == request.ParentId && request.Services.Select(s => s.ServiceId).Contains(so.ServiceId))
                .ToListAsync();

            if (!serviceOrders.Any())
            {
                return NotFound(new { message = "No services found for the given ParentId" });
            }

            //Tính tổng tiền của đơn hàng
            //decimal totalAmount = serviceOrders.Sum(so => (decimal)so.TotalPrice);
            decimal totalAmount = serviceOrders.Sum(so => (decimal)so.Service.ServicePrice * so.Quantity);


            //Tạo danh sách các dịch vụ để gửi lên PayOS
            var serviceList = serviceOrders.Select(so => new ItemData(
                name: so.Service.ServiceName,
                quantity: 1,
                price: (int)so.TotalPrice
            )).ToList();

            //Lấy thông tin cấu hình PayOS từ appsettings.json
            var clientId = _config["PayOS:ClientId"];
            var apiKey = _config["PayOS:ApiKey"];
            var checksumKey = _config["PayOS:ChecksumKey"];

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(checksumKey))
            {
                return StatusCode(500, new { message = "Missing PayOS configuration" });
            }

            var payOS = new PayOS(clientId, apiKey, checksumKey);

            // Lay ma don hang tu db
            long orderCode = (long)serviceOrders.First().OrderCode;

            // Tạo dữ liệu thanh toán gửi lên PayOS
            var paymentRequest = new PaymentData(
                orderCode: orderCode,
                amount: (int)totalAmount,
                description: request.Description,
                items: serviceList,
                returnUrl: request.ReturnUrl,
                cancelUrl: request.CancelUrl
            );

            try
            {
                //Gửi yêu cầu tạo link thanh toán tới PayOS
                var response = await payOS.createPaymentLink(paymentRequest);
                if (response == null || string.IsNullOrEmpty(response.checkoutUrl))
                {
                    return StatusCode(500, new { message = "Failed to get checkout URL from PayOS" });
                }

                /*//Lưu thông tin thanh toán vào DB trước khi trả về URL
                var payment = new Payment
                {
                    PaymentId = Guid.NewGuid(),
                    Amount = totalAmount,
                    PaymentDate = DateTime.UtcNow,
                    PaymentMethod = "PayOS",
                    PaymentStatus = PaymentStatus.Pending,
                    PaymentUrl = response.checkoutUrl,
                    ServiceOrderId = serviceOrders.First().ServiceOrderId,
                    TransactionId = orderCode.ToString()
                };

                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();*/

                //Trả về URL để người dùng thanh toán
                return Ok(new { paymentUrl = response.checkoutUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to create payment", error = ex.Message });
            }
        }
        /// <summary>
        /// Lấy đơn hàng theo ParentId
        /// </summary>
        [HttpGet("{parentId}")]
        public async Task<IActionResult> GetServiceOrderByParentId(Guid parentId)
        {
            var serviceOrder = await _serviceOrderService.GetServiceOrderByParentId(parentId);
            if (serviceOrder == null) return NotFound(new { message = "Order not found" });

            return Ok(serviceOrder);
        }
        /// <summary>
        /// Kiểm tra trạng thái thanh toán từ PayOS bằng OrderCode
        /// </summary>
        [HttpGet("check-status/{orderCode}")]
        public async Task<IActionResult> CheckPaymentStatus(long orderCode)
        {
            string? status = await _serviceOrderService.GetPaymentStatusFromPayOS(orderCode);
            if (status == null) return NotFound(new { message = "Payment not found" });

            return Ok(new { orderCode, status });
        }

        /// <summary>
        /// Kiểm tra và cập nhật trạng thái thanh toán từ PayOS
        /// </summary>
        [HttpPost("update-status/{parentId}")]
        public async Task<IActionResult> UpdateServiceOrderStatus(Guid parentId)
        {
            try
            {
                await _serviceOrderService.UpdateServiceOrderStatus(parentId);
                return Ok(new { message = "Order status updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        
    }
    public class Item
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; } // PayOS yêu cầu kiểu long

        public Item(string name, int quantity, long price)
        {
            Name = name;
            Quantity = quantity;
            Price = price;
        }
    }
    public class CreateServiceOrderRequest
    {
        public Guid ParentId { get; set; }
        public int ServiceId { get; set; }
        public int Quantity { get; set; }
    }

    public class CreatePaymentRequest
    {
        public Guid ServiceOrderId { get; set; }
        public string PaymentMethod { get; set; }
    }

    public class PaymentRequestModel
    {
        public Guid ParentId { get; set; } // Để xác định người đặt hàng
        public string Description { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
        public List<ServiceOrderModel> Services { get; set; }
    }

    public class ServiceOrderModel
    {
        public int ServiceId { get; set; }
        public int Quantity { get; set; }
        public float TotalPrice { get; set; }
    }



}
