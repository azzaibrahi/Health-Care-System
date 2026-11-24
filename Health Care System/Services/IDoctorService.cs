using Health_Care_System.Dtos.DoctorDtos;
using Health_Care_System.Results;

namespace Health_Care_System.Services;

/// <summary>
/// Read operations on doctors.
/// </summary>
public interface IDoctorService
{
    Task<GenericResult<IEnumerable<GetDoctorResponse>>> GetAllAsync();

    Task<GenericResult<GetDoctorResponse>> GetByIdAsync(int id);
}
