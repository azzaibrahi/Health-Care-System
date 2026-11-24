using AutoMapper;
using Health_Care_System.Dtos.AppointmentDtos;
using Health_Care_System.Entities;

namespace Health_Care_System.Helper;

/// <summary>
/// Flattens an <see cref="Appointment"/> and the two navigation properties
/// into a single response DTO.
/// </summary>
public class AppointmentProfile : Profile
{
    public AppointmentProfile()
    {
        CreateMap<Appointment, GetAppointmentResponse>()
            .ForMember(
                destination => destination.PatientName,
                options => options.MapFrom(source => source.Patient.Name))
            .ForMember(
                destination => destination.DoctorName,
                options => options.MapFrom(source => source.Doctor.Name));
    }
}
