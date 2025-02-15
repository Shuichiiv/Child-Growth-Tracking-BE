using DTOs_BE.DoctorDTOs;

namespace Services_BE.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendFeedbackEmailAsync(EmailDto emailDto);
    }
}
