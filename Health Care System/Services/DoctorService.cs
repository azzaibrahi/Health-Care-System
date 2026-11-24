using AutoMapper;
using Health_Care_System.Dtos.DoctorDtos;
using Health_Care_System.Entities;
using Health_Care_System.Repositories;
using Health_Care_System.Results;

namespace Health_Care_System.Services;

/// <summary>
/// Reads doctors through the generic repository and shapes the answer with AutoMapper.
/// </summary>
public class DoctorService(IGenericRepository<Doctor> repository, IMapper mapper) : IDoctorService
{
    public async Task<GenericResult<IEnumerable<GetDoctorResponse>>> GetAllAsync()
    {
        var entities = await repository.GetAllAsync();
        var response = mapper.Map<IEnumerable<GetDoctorResponse>>(entities);

        return GenericResult<IEnumerable<GetDoctorResponse>>.Success(response);
    }

    public async Task<GenericResult<GetDoctorResponse>> GetByIdAsync(int id)
    {
        var entity = await repository.GetByIdAsync(id);

        if (entity is null)
        {
            return GenericResult<GetDoctorResponse>.Failure($"Doctor with Id = {id} is not found.");
        }

        var response = mapper.Map<GetDoctorResponse>(entity);

        return GenericResult<GetDoctorResponse>.Success(response);
    }
}
