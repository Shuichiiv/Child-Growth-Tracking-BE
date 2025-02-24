using AutoMapper;
using DataObjects_BE.Entities;
using DTOs_BE.AppointmentDtos;
using Repositories_BE.Interfaces;
using Services_BE.Interfaces;

namespace Services_BE.Services
{
    public class AppointmentService: IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync()
        {
            var appointments = await _appointmentRepository.GetAllAppointmentsAsync();
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<AppointmentDto?> GetAppointmentByIdAsync(Guid appointmentId)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
            return appointment != null ? _mapper.Map<AppointmentDto>(appointment) : null;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorIdAsync(Guid doctorId)
        {
            var appointments = await _appointmentRepository.GetAppointmentsByDoctorIdAsync(doctorId);
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByParentIdAsync(Guid parentId)
        {
            var appointments = await _appointmentRepository.GetAppointmentsByParentIdAsync(parentId);
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<bool> CreateAppointmentAsync(AppointmentCreateDto appointmentDto)
        {
            var appointment = _mapper.Map<Appointment>(appointmentDto);
            await _appointmentRepository.AddAppointmentAsync(appointment);
            return true;
        }

        public async Task<bool> UpdateAppointmentAsync(Guid appointmentId, AppointmentUpdateDto appointmentDto)
        {
            var existingAppointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
            if (existingAppointment == null) return false;

            _mapper.Map(appointmentDto, existingAppointment);
            await _appointmentRepository.UpdateAppointmentAsync(existingAppointment);
            return true;
        }

        public async Task<bool> DeleteAppointmentAsync(Guid appointmentId)
        {
            if (!await _appointmentRepository.AppointmentExistsAsync(appointmentId)) return false;
            
            await _appointmentRepository.DeleteAppointmentAsync(appointmentId);
            return true;
        }
        public async Task<bool> ConfirmAppointmentAsync(Guid appointmentId)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
            if (appointment == null) return false;

            appointment.Status = AppointmentStatus.Confirmed;
            appointment.UpdatedAt = DateTime.UtcNow;
            return await _appointmentRepository.UpdateAppointmentAsyncN(appointment);
        }

        public async Task<bool> CancelAppointmentAsync(Guid appointmentId)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
            if (appointment == null) return false;

            appointment.Status = AppointmentStatus.Canceled;
            appointment.UpdatedAt = DateTime.UtcNow;
            return await _appointmentRepository.UpdateAppointmentAsyncN(appointment);
        }
    }
}