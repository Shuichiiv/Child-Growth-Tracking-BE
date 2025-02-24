using System.Text;
using QRCoder;
using Services_BE.Interfaces;

namespace Services_BE.Services
{

    public class VietQRService: IVietQRService
    {
        private const string BANK_ID = "970423"; // TPBank
        private const string CURRENCY = "704"; // VND

        private string GenerateVietQRString(string accountNumber, string accountName, decimal amount, string message)
        {
            string amountFormatted = amount > 0 ? $"{amount:0.00}" : "";
            string template = "00020101021238560010A00000072701230006970423" +
                              $"0118{accountNumber}520400005303{CURRENCY}540{amountFormatted}" +
                              $"5802VN5913{accountName}6008HANOI6213{message}6304{GenerateCRC16()}";
            return template;
        }

        public byte[] GenerateQR(string accountNumber, string accountName, decimal amount, string message)
        {
            string qrData = GenerateVietQRString(accountNumber, accountName, amount, message);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }

        private string GenerateCRC16()
        {
            byte[] bytes = Encoding.UTF8.GetBytes("VietQR");
            ushort crc = 0xFFFF;
            foreach (byte b in bytes)
            {
                crc ^= (ushort)(b << 8);
                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x8000) != 0)
                        crc = (ushort)((crc << 1) ^ 0x1021);
                    else
                        crc <<= 1;
                }
            }
            return crc.ToString("X4");
        }
    }
}