namespace Health_Care_System.Dtos.PatientDtos;

/// <summary>
/// A patient as the API returns it. Audit fields and the appointment
/// collection are deliberately left out.
/// </summary>
public class GetPatientResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateOnly DateOfBirth { get; set; }
}
