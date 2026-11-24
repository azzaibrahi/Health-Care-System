namespace Health_Care_System.Dtos.AppointmentDtos;

/// <summary>
/// An appointment as the API returns it. The composite key is exposed as
/// patient id, doctor id and date, and the names are flattened so a caller
/// does not need a second request.
/// </summary>
public class GetAppointmentResponse
{
    public int PatientId { get; set; }

    public string PatientName { get; set; } = string.Empty;

    public int DoctorId { get; set; }

    public string DoctorName { get; set; } = string.Empty;

    public DateTime AppointmentDate { get; set; }
}
