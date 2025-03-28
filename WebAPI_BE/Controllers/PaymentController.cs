using DataObjects_BE;
using Microsoft.AspNetCore.Mvc;
using DataObjects_BE.Entities;
using DTOs_BE.PaymentDTOs;
using Microsoft.EntityFrameworkCore;
using Net.payOS;
using Net.payOS.Types;
using Services_BE.Interfaces;
using Services_BE.Services;


namespace WebAPI_BE.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly ServiceOrderServiceP _serviceOrderService;
        private readonly IPaymentService _paymentService1;

        public PaymentController(ServiceOrderServiceP serviceOrderService,
             IPaymentService paymentService1)
        {
            _serviceOrderService = serviceOrderService;
            _paymentService1 = paymentService1;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequestModel request)
        {
            var (success, message, paymentUrl) = await _paymentService1.CreatePaymentAsync(request);
            if (!success)
            {
                return BadRequest(new { message });
            }
            return Ok(new { paymentUrl });
        }
        /*[HttpPost("create")]
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

                //Lưu thông tin thanh toán vào DB trước khi trả về URL
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
                await _context.SaveChangesAsync();

                //Trả về URL để người dùng thanh toán
                return Ok(new { paymentUrl = response.checkoutUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to create payment", error = ex.Message });
            }
        }*/

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



}
