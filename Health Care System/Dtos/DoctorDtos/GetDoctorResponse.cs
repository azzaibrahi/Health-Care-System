namespace Health_Care_System.Dtos.DoctorDtos;

/// <summary>
/// A doctor as the API returns it.
/// </summary>
public class GetDoctorResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Specialization { get; set; } = string.Empty;
}
