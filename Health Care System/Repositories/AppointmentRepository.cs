using Health_Care_System.Data;
using Health_Care_System.Entities;
using Microsoft.EntityFrameworkCore;

namespace Health_Care_System.Repositories;

/// <summary>
/// Read/write access to <see cref="Appointment"/> through its composite key.
/// The patient and the doctor are eager-loaded because the response DTO
/// exposes both names.
/// </summary>
public class AppointmentRepository(ApplicationDbContext dbContext) : IAppointmentRepository
{
    public async Task<IEnumerable<Appointment>> GetAllAsync()
        => await dbContext.Appointments
            .Include(appointment => appointment.Patient)
            .Include(appointment => appointment.Doctor)
            .ToListAsync();

    public async Task<Appointment?> GetByIdAsync(
        int patientId,
        int doctorId,
        DateTime appointmentDate)
        => await dbContext.Appointments
            .Include(appointment => appointment.Patient)
            .Include(appointment => appointment.Doctor)
            .FirstOrDefaultAsync(appointment =>
                appointment.PatientId == patientId
                && appointment.DoctorId == doctorId
                && appointment.AppointmentDate == appointmentDate);

    public async Task<Appointment> CreateAsync(Appointment entity)
    {
        await dbContext.Appointments.AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int patientId, int doctorId, DateTime appointmentDate)
    {
        var entity = await GetByIdAsync(patientId, doctorId, appointmentDate);

        if (entity is null)
        {
            return false;
        }

        dbContext.Appointments.Remove(entity);
        await dbContext.SaveChangesAsync();
        return true;
    }
}
