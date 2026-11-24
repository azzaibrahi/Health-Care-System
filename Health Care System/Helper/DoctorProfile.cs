using AutoMapper;
using Health_Care_System.Dtos.DoctorDtos;
using Health_Care_System.Entities;

namespace Health_Care_System.Helper;

/// <summary>
/// Maps <see cref="Doctor"/> to and from its DTOs.
/// </summary>
public class DoctorProfile : Profile
{
    public DoctorProfile()
    {
        CreateMap<Doctor, GetDoctorResponse>().ReverseMap();
    }
}
