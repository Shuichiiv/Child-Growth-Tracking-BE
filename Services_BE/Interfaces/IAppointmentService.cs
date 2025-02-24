using DTOs_BE.AppointmentDtos;

namespace Services_BE.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync();
        Task<AppointmentDto?> GetAppointmentByIdAsync(Guid appointmentId);
        Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorIdAsync(Guid doctorId);
        Task<IEnumerable<AppointmentDto>> GetAppointmentsByParentIdAsync(Guid parentId);
        Task<bool> CreateAppointmentAsync(AppointmentCreateDto appointmentDto);
        Task<bool> UpdateAppointmentAsync(Guid appointmentId, AppointmentUpdateDto appointmentDto);
        Task<bool> DeleteAppointmentAsync(Guid appointmentId);
        Task<bool> ConfirmAppointmentAsync(Guid appointmentId);
        Task<bool> CancelAppointmentAsync(Guid appointmentId);
    }
}