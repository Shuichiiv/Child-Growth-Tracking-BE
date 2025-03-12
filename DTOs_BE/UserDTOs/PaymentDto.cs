namespace DTOs_BE.UserDTOs
{
    public class PayOSResponse
    {
        public string payment_url { get; set; } // URL để redirect người dùng đến trang thanh toán
        public string order_code { get; set; } // Mã đơn hàng (ID thanh toán)
        public string status { get; set; } // Trạng thái thanh toán
    }
    public class PayOSCallbackModel
    {
        public Guid OrderCode { get; set; } // Mã đơn hàng (tương ứng với PaymentId)
        public string Status { get; set; } // Trạng thái thanh toán: "Success" hoặc "Failed"
        public string TransactionId { get; set; } // Mã giao dịch từ PayOS
    }

    
    public class PaymentDto
    {


    }
}

