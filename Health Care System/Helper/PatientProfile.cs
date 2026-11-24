using AutoMapper;
using Health_Care_System.Dtos.PatientDtos;
using Health_Care_System.Entities;

namespace Health_Care_System.Helper;

/// <summary>
/// Maps <see cref="Patient"/> to and from its DTOs.
/// </summary>
public class PatientProfile : Profile
{
    public PatientProfile()
    {
        CreateMap<Patient, GetPatientResponse>().ReverseMap();
    }
}
