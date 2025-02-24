namespace Services_BE.Interfaces;

public interface IVietQRService
{
    byte[] GenerateQR(string accountNumber, string accountName, decimal amount, string message);
}