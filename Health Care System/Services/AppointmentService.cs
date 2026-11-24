using AutoMapper;
using Health_Care_System.Dtos.AppointmentDtos;
using Health_Care_System.Repositories;
using Health_Care_System.Results;

namespace Health_Care_System.Services;

/// <summary>
/// Reads appointments through <see cref="IAppointmentRepository"/> and flattens
/// the patient and doctor names with AutoMapper.
/// </summary>
public class AppointmentService(IAppointmentRepository repository, IMapper mapper) : IAppointmentService
{
    public async Task<GenericResult<IEnumerable<GetAppointmentResponse>>> GetAllAsync()
    {
        var entities = await repository.GetAllAsync();
        var response = mapper.Map<IEnumerable<GetAppointmentResponse>>(entities);

        return GenericResult<IEnumerable<GetAppointmentResponse>>.Success(response);
    }

    public async Task<GenericResult<GetAppointmentResponse>> GetByIdAsync(
        int patientId,
        int doctorId,
        DateTime appointmentDate)
    {
        var entity = await repository.GetByIdAsync(patientId, doctorId, appointmentDate);

        if (entity is null)
        {
            return GenericResult<GetAppointmentResponse>.Failure(
                "Appointment is not found.");
        }

        var response = mapper.Map<GetAppointmentResponse>(entity);

        return GenericResult<GetAppointmentResponse>.Success(response);
    }
}
