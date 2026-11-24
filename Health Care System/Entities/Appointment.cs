namespace Health_Care_System.Entities;

/// <summary>
/// Join entity between <see cref="Patient"/> and <see cref="Doctor"/>.
/// The composite key allows the same patient and doctor to meet on different dates,
/// while still preventing an exact duplicate appointment.
/// </summary>
public class Appointment
{
    public int PatientId { get; set; }

    public Patient Patient { get; set; } = null!;

    public int DoctorId { get; set; }

    public Doctor Doctor { get; set; } = null!;

    public DateTime AppointmentDate { get; set; }
}
