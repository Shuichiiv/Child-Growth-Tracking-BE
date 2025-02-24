using Microsoft.AspNetCore.Mvc;
using Services_BE.Interfaces;

namespace WebAPI_BE.Controllers
{
    public class PaymentController : ControllerBase
    {
        private readonly IVietQRService _vietQRService;

        public PaymentController(IVietQRService vietQRService)
        {
            _vietQRService = vietQRService;
        }

        [HttpGet("generate")]
        public IActionResult GenerateQRCode(string accountNumber, string accountName, decimal amount, string message)
        {
            var qrCode = _vietQRService.GenerateQR(accountNumber, accountName, amount, message);
            return File(qrCode, "image/png");
        }
    }
}   