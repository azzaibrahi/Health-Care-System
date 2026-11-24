using Health_Care_System.Dtos.PatientDtos;
using Health_Care_System.Results;

namespace Health_Care_System.Services;

/// <summary>
/// Read operations on patients. Writes arrive in a later session.
/// </summary>
public interface IPatientService
{
    Task<GenericResult<IEnumerable<GetPatientResponse>>> GetAllAsync();

    Task<GenericResult<GetPatientResponse>> GetByIdAsync(int id);
}
