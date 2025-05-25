using cw9.Models;
using Microsoft.EntityFrameworkCore;

namespace cw9.Data;

public class AppDbContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Define keys
        modelBuilder.Entity<Patient>().HasKey(p => p.IdPatient);
        modelBuilder.Entity<Doctor>().HasKey(d => d.IdDoctor);
        modelBuilder.Entity<Medicament>().HasKey(m => m.IdMedicament);
        modelBuilder.Entity<Prescription>().HasKey(p => p.IdPrescription);
        modelBuilder.Entity<PrescriptionMedicament>().HasKey(pm => new { pm.IdPrescription, pm.IdMedicament });

        // Relationships
        modelBuilder.Entity<PrescriptionMedicament>()
            .HasOne(pm => pm.Prescription)
            .WithMany(p => p.PrescriptionMedicaments)
            .HasForeignKey(pm => pm.IdPrescription);

        modelBuilder.Entity<PrescriptionMedicament>()
            .HasOne(pm => pm.Medicament)
            .WithMany(m => m.PrescriptionMedicaments)
            .HasForeignKey(pm => pm.IdMedicament);

        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Doctor)
            .WithMany(d => d.Prescriptions)
            .HasForeignKey(p => p.IdDoctor)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Patient)
            .WithMany(pat => pat.Prescriptions)
            .HasForeignKey(p => p.IdPatient)
            .OnDelete(DeleteBehavior.Restrict);

        // Constraints
        modelBuilder.Entity<Patient>()
            .Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Doctor>()
            .Property(d => d.Email)
            .HasMaxLength(100);

        // initial data

        modelBuilder.Entity<Doctor>().HasData(new Doctor
        {
            IdDoctor = 1,
            FirstName = "Anna",
            LastName = "Kowalska",
            Email = "anna.kowalska@hospital.com"
        });

        modelBuilder.Entity<Patient>().HasData(new Patient
        {
            IdPatient = 1,
            FirstName = "Jan",
            LastName = "Nowak",
            Birthdate = new DateTime(1995, 10, 10)
        });

        modelBuilder.Entity<Medicament>().HasData(new Medicament
        {
            IdMedicament = 1,
            Name = "Ibuprofen",
            Description = "Painkiller",
            Type = "Tablet"
        });

        modelBuilder.Entity<Prescription>().HasData(new Prescription
        {
            IdPrescription = 1,
            Date = new DateTime(2024, 5, 25),
            DueDate = new DateTime(2024, 6, 5),
            IdDoctor = 1,
            IdPatient = 1
        });

        modelBuilder.Entity<PrescriptionMedicament>().HasData(new PrescriptionMedicament
        {
            IdPrescription = 1,
            IdMedicament = 1,
            Dose = 2,
            Details = "Take twice daily"
        });
        
    }
}
