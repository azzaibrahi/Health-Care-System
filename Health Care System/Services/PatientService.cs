using AutoMapper;
using Health_Care_System.Dtos.PatientDtos;
using Health_Care_System.Entities;
using Health_Care_System.Repositories;
using Health_Care_System.Results;

namespace Health_Care_System.Services;

/// <summary>
/// Reads patients through the generic repository and shapes the answer with AutoMapper.
/// The service never touches the <c>DbContext</c> directly.
/// </summary>
public class PatientService(IGenericRepository<Patient> repository, IMapper mapper) : IPatientService
{
    public async Task<GenericResult<IEnumerable<GetPatientResponse>>> GetAllAsync()
    {
        var entities = await repository.GetAllAsync();
        var response = mapper.Map<IEnumerable<GetPatientResponse>>(entities);

        return GenericResult<IEnumerable<GetPatientResponse>>.Success(response);
    }

    public async Task<GenericResult<GetPatientResponse>> GetByIdAsync(int id)
    {
        var entity = await repository.GetByIdAsync(id);

        if (entity is null)
        {
            return GenericResult<GetPatientResponse>.Failure($"Patient with Id = {id} is not found.");
        }

        var response = mapper.Map<GetPatientResponse>(entity);

        return GenericResult<GetPatientResponse>.Success(response);
    }
}
