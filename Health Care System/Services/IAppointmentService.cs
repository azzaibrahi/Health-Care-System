using Health_Care_System.Dtos.AppointmentDtos;
using Health_Care_System.Results;

namespace Health_Care_System.Services;

/// <summary>
/// Read operations on appointments, addressed by the composite key.
/// </summary>
public interface IAppointmentService
{
    Task<GenericResult<IEnumerable<GetAppointmentResponse>>> GetAllAsync();

    Task<GenericResult<GetAppointmentResponse>> GetByIdAsync(
        int patientId,
        int doctorId,
        DateTime appointmentDate);
}
