using Microsoft.AspNetCore.Mvc;
using Services_BE.Interfaces;
using Services_BE.Services;

namespace WebAPI_BE.Controllers
{
    public class PaymentController : ControllerBase
    {
        private readonly IVietQRService _vietQRService;
        private readonly PaymentService _paymentService;

        public PaymentController(IVietQRService vietQRService,PaymentService paymentService)
        {
            _vietQRService = vietQRService;
            _paymentService = paymentService;
        }

        [HttpGet("generate")]
        public IActionResult GenerateQRCode(string accountNumber, string accountName, decimal amount, string message)
        {
            var qrCode = _vietQRService.GenerateQR(accountNumber, accountName, amount, message);
            return File(qrCode, "image/png");
        }
        
        [HttpPost("generate-qr")]
        public async Task<IActionResult> GenerateVietQR(Guid serviceOrderId, decimal amount)
        {
            var payment = await _paymentService.GenerateVietQRAsync(serviceOrderId, amount);
            var qrUrl = $"https://img.vietqr.io/image/970422-0123456789-print.png?amount={amount}&addInfo={serviceOrderId}";

            return Ok(new { qrUrl, payment });
        }

        [HttpPost("update-payment-status")]
        public async Task<IActionResult> UpdatePaymentStatus(Guid serviceOrderId, bool isPaid)
        {
            var success = await _paymentService.ConfirmPaymentAsync(serviceOrderId, isPaid);
            if (!success) return NotFound("Không tìm thấy giao dịch.");
            return Ok("Cập nhật trạng thái thành công.");
        }

        [HttpGet("check-payment-status")]
        public async Task<IActionResult> CheckPaymentStatus(Guid serviceOrderId)
        {
            var status = await _paymentService.CheckPaymentStatusAsync(serviceOrderId);
            return Ok(new { serviceOrderId, status });
        }
    }
}   