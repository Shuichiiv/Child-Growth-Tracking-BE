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
        private readonly ICurrentTime _currentTime;

        public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper, ICurrentTime currentTime)
        {
            _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
            _mapper = mapper;
            _currentTime = currentTime ?? throw new ArgumentNullException(nameof(currentTime));
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
            if (appointmentDto == null) throw new ArgumentNullException(nameof(appointmentDto));

            try
            {
                var appointment = _mapper.Map<Appointment>(appointmentDto);
                
                var vietNamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var nowUtc7 = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietNamTimeZone);

                if (appointmentDto.ScheduledTime == default)
                {
                    throw new Exception("ScheduledTime is required.");
                }
                else if (appointmentDto.ScheduledTime < DateTime.UtcNow)
                {
                    throw new Exception("Thời gian tạo trong quá khứ, không thể tạo lịch hẹn.");
                }

                appointment.ScheduledTime = appointmentDto.ScheduledTime;
                appointment.CreatedAt = nowUtc7;

                await _appointmentRepository.AddAppointmentAsync(appointment);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception($"Lỗi khi tạo lịch hẹn: {e.Message}", e);
            }
        }
        
        public async Task<bool> UpdateAppointmentAsync(Guid appointmentId, AppointmentUpdateDto appointmentDto)
        {
            try
            {
                var existingAppointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
                if (existingAppointment == null) 
                {
                    Console.WriteLine($"Không tìm thấy lịch hẹn với ID: {appointmentId}");
                    return false;
                }

                if (appointmentDto.ScheduledTime == default)
                {
                    throw new Exception("ScheduledTime is required.");
                }
                else if (appointmentDto.ScheduledTime < DateTime.UtcNow)
                {
                    throw new Exception("Không thể cập nhật với thời gian trong quá khứ.");
                }

                Console.WriteLine($"Before Mapping: {existingAppointment.ScheduledTime}");
        
                _mapper.Map(appointmentDto, existingAppointment);

                Console.WriteLine($"After Mapping: {existingAppointment.ScheduledTime}");
                
                TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                existingAppointment.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);


                await _appointmentRepository.UpdateAppointmentAsync(existingAppointment);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Lỗi khi cập nhật lịch hẹn: {e.Message}");
                throw new Exception("Lỗi khi cập nhật lịch hẹn", e);
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