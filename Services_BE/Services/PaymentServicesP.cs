using System.Net.Http.Json;
using System.Text.Json;
using DataObjects_BE;
using DataObjects_BE.Entities;

namespace Services_BE.Services;

public class PaymentServicesP
{
    private readonly SWP391G3DbContext _context;
    private readonly HttpClient _httpClient;

    public PaymentServicesP(SWP391G3DbContext context, HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    public async Task<Payment> CreatePaymentAsync(Guid serviceOrderId, string paymentMethod)
    {
        var serviceOrder = await _context.ServiceOrders.FindAsync(serviceOrderId);
        if (serviceOrder == null) throw new Exception("ServiceOrder not found");

        var payment = new Payment
        {
            PaymentId = Guid.NewGuid(),
            ServiceOrderId = serviceOrderId,
            Amount = (decimal)serviceOrder.TotalPrice,
            PaymentMethod = paymentMethod,
            PaymentDate = DateTime.UtcNow,
            PaymentStatus = PaymentStatus.Pending
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        return payment;
    }

    public async Task<string> GetPayOSPaymentUrlAsync(Guid paymentId)
    {
        var payment = await _context.Payments.FindAsync(paymentId);
        if (payment == null) throw new Exception("Payment not found");

        // 🔍 Đảm bảo Amount là số nguyên
        int amount = (int)(payment.Amount * 100); // Chuyển thành đơn vị VNĐ (nếu PayOS yêu cầu)

        var payOsRequest = new
        {
            amount = amount, // Chuyển đổi amount thành số nguyên
            order_id = payment.PaymentId.ToString(),
            description = "Thanh toán dịch vụ",
            return_url = "https://localhost:7190/payment-success",
            cancel_url = "https://localhost:7190/payment-failed",
            /*merchant_id = "YOUR_MERCHANT_ID_HERE", // ⚠️ Thêm Merchant ID
            public_key = "YOUR_PUBLIC_KEY_HERE"    // ⚠️ Thêm Public Key*/
        };

        var httpClient = new HttpClient();
    
        // 🔥 DEBUG: In ra URL API
        // 🛠️ THÊM API KEY & CLIENT ID VÀO HEADER
        httpClient.DefaultRequestHeaders.Add("x-client-id", "9248e8d9-9110-4c4d-96e1-f61cec695c91");
        httpClient.DefaultRequestHeaders.Add("x-api-key", "6fda83ce-45f3-4071-a327-7ed27f5e3fca");
        string payOsUrl = "https://api-merchant.payos.vn/v2/payment-requests";
        Console.WriteLine($"🔍 Sending request to: {payOsUrl}");
    
        try
        {
            var response = await httpClient.PostAsJsonAsync(payOsUrl, payOsRequest);
        
            // 🔥 DEBUG: In ra response status
            Console.WriteLine($"🔍 Response status: {response.StatusCode}");

            var responseData = await response.Content.ReadFromJsonAsync<PayOSResponse>();

            if (responseData?.payment_url == null)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ PayOS error response: {errorMessage}");
                throw new Exception("Failed to get payment URL: " + errorMessage);
            }

            payment.PaymentUrl = responseData.payment_url;
            await _context.SaveChangesAsync();

            return responseData.payment_url;
        }
        catch (HttpRequestException httpEx)
        {
            Console.WriteLine($"❌ HTTP request error: {httpEx.Message}");
            throw new Exception("HTTP request error: " + httpEx.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Unknown error: {ex.Message}");
            throw;
        }
    }

    public class PayOSResponse
    {
        public string payment_url { get; set; }
    }

}