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
