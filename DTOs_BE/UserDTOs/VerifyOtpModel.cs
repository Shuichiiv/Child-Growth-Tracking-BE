namespace DTOs_BE.UserDTOs
{
    public class VerifyOtpModel
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }

    public class OtpInfo()
    {
        public string Email { get; set; }
        public string OtpCode { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public bool IsUsed { get; set; }
        public DateTime? OtpCreatedAt { get; set; }
    }
    
    public class ResendOtpRequestModel
    {
        public string Email { get; set; }
    }
}