using Health_Care_System.Entities;

namespace Health_Care_System.Repositories;

/// <summary>
/// Appointment has a composite key (<c>PatientId</c>, <c>DoctorId</c>, <c>AppointmentDate</c>)
/// and no <c>Id</c> column, so it cannot use <see cref="IGenericRepository{T}"/>
/// and needs its own contract.
/// </summary>
public interface IAppointmentRepository
{
    Task<IEnumerable<Appointment>> GetAllAsync();

    Task<Appointment?> GetByIdAsync(int patientId, int doctorId, DateTime appointmentDate);

    Task<Appointment> CreateAsync(Appointment entity);

    Task<bool> DeleteAsync(int patientId, int doctorId, DateTime appointmentDate);
}
