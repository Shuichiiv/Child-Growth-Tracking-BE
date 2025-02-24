using DataObjects_BE;
using DataObjects_BE.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories_BE.Interfaces;

namespace Repositories_BE.Repositories
{
    public class AppointmentRepository: IAppointmentRepository
    {
        private readonly SWP391G3DbContext _context;

        public AppointmentRepository(SWP391G3DbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Parent)
                .Include(a => a.Child)
                .ToListAsync();
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(Guid appointmentId)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Parent)
                .Include(a => a.Child)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(Guid doctorId)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .Include(a => a.Parent)
                .Include(a => a.Child)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByParentIdAsync(Guid parentId)
        {
            return await _context.Appointments
                .Where(a => a.ParentId == parentId)
                .Include(a => a.Doctor)
                .Include(a => a.Child)
                .ToListAsync();
        }

        public async Task AddAppointmentAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAppointmentAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAppointmentAsync(Guid appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> AppointmentExistsAsync(Guid appointmentId)
        {
            return await _context.Appointments.AnyAsync(a => a.AppointmentId == appointmentId);
        }
        public async Task<Appointment> GetAppointmentByIdAsyncN(Guid appointmentId)
        {
            return await _context.Appointments.FindAsync(appointmentId);
        }

        public async Task<bool> UpdateAppointmentAsyncN(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}