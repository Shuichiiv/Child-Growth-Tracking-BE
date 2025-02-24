using DataObjects_BE.Entities;

namespace Repositories_BE.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment?> GetAppointmentByIdAsync(Guid appointmentId);
        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(Guid doctorId);
        Task<IEnumerable<Appointment>> GetAppointmentsByParentIdAsync(Guid parentId);
        Task AddAppointmentAsync(Appointment appointment);
        Task UpdateAppointmentAsync(Appointment appointment);
        Task DeleteAppointmentAsync(Guid appointmentId);
        Task<bool> AppointmentExistsAsync(Guid appointmentId);
        
        Task<Appointment> GetAppointmentByIdAsyncN(Guid appointmentId);
        Task<bool> UpdateAppointmentAsyncN(Appointment appointment);
    }
}