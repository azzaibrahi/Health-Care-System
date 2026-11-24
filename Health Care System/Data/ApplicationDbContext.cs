using Health_Care_System.Entities;
using Microsoft.EntityFrameworkCore;

namespace Health_Care_System.Data;

/// <summary>
/// EF Core session with the Health Care database.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();

    public DbSet<Doctor> Doctors => Set<Doctor>();

    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.Property(patient => patient.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasMany(patient => patient.Appointments)
                .WithOne(appointment => appointment.Patient)
                .HasForeignKey(appointment => appointment.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.Property(doctor => doctor.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(doctor => doctor.Specialization)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasMany(doctor => doctor.Appointments)
                .WithOne(appointment => appointment.Doctor)
                .HasForeignKey(appointment => appointment.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            // The same patient and doctor may meet again on another date.
            entity.HasKey(appointment => new
            {
                appointment.PatientId,
                appointment.DoctorId,
                appointment.AppointmentDate
            });

            entity.Property(appointment => appointment.AppointmentDate)
                .IsRequired();

            // The composite key starts with PatientId; this index serves doctor lookups.
            entity.HasIndex(appointment => appointment.DoctorId);
        });
    }
}
