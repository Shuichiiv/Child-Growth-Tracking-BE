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
            _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync()
        {
            try
            {
                var appointments = await _appointmentRepository.GetAllAppointmentsAsync();
                return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving appointments", ex);
            }
        }

        public async Task<AppointmentDto?> GetAppointmentByIdAsync(Guid appointmentId)
        {
            try
            {
                var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
                return appointment != null ? _mapper.Map<AppointmentDto>(appointment) : null;
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving appointments", e);
            }
           
        }

        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorIdAsync(Guid doctorId)
        {
            try
            {
                var appointments = await _appointmentRepository.GetAppointmentsByDoctorIdAsync(doctorId);
                return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving appointments", e);
            }
           
        }

        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByParentIdAsync(Guid parentId)
        {
            try
            {
                var appointments = await _appointmentRepository.GetAppointmentsByParentIdAsync(parentId);
                return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving appointments", e);
            }
           
        }

        public async Task<bool> CreateAppointmentAsync(AppointmentCreateDto appointmentDto)
        {
            try
            {
                var appointment = _mapper.Map<Appointment>(appointmentDto);
                
                if (appointmentDto.ScheduledTime == default)
                {
                    throw new Exception("ScheduledTime is required.");
                }
                
                else if (appointmentDto.ScheduledTime < DateTime.Now)
                {
                    throw new Exception("Thoi gian tao trong qua khu, khong the tao");
                }
                {
                    appointment.ScheduledTime = appointmentDto.ScheduledTime;
                }
                
                appointment.CreatedAt = DateTime.UtcNow;

                await _appointmentRepository.AddAppointmentAsync(appointment);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while adding appointments", e);
            }
        }



        public async Task<bool> UpdateAppointmentAsync(Guid appointmentId, AppointmentUpdateDto appointmentDto)
        {
            try
            {
                var existingAppointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
                if (existingAppointment == null) return false;

                _mapper.Map(appointmentDto, existingAppointment);
                await _appointmentRepository.UpdateAppointmentAsync(existingAppointment);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while updating appointments", e);
            }
            
        }

        public async Task<bool> DeleteAppointmentAsync(Guid appointmentId)
        {
            try
            {
                if (!await _appointmentRepository.AppointmentExistsAsync(appointmentId)) return false;
            
                await _appointmentRepository.DeleteAppointmentAsync(appointmentId);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while deleting appointments", e);
            }
            
        }
        public async Task<bool> ConfirmAppointmentAsync(Guid appointmentId)
        {
            try
            {
                var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
                if (appointment == null) return false;

                appointment.Status = AppointmentStatus.Confirmed;
                appointment.UpdatedAt = DateTime.UtcNow;
                return await _appointmentRepository.UpdateAppointmentAsyncN(appointment);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while confirming appointments", e);
            }
            
        }

        public async Task<bool> CancelAppointmentAsync(Guid appointmentId)
        {
            try
            {
                var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
                if (appointment == null) return false;

                appointment.Status = AppointmentStatus.Canceled;
                appointment.UpdatedAt = DateTime.UtcNow;
                return await _appointmentRepository.UpdateAppointmentAsyncN(appointment);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while cancelling appointments", e);
            }
            
        }
    }
}