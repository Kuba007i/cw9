using Microsoft.EntityFrameworkCore;
using cw9.Data;
using cw9.DTOs;
using cw9.Models;

namespace cw9.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly AppDbContext _context;

    public PrescriptionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(bool IsSuccess, string? ErrorMessage, int? PrescriptionId)> AddPrescriptionAsync(AddPrescriptionRequestDTO dto)
    {
        if (dto.Medicaments.Count > 10)
            return (false, "Prescription cannot contain more than 10 medicaments.", null);

        if (dto.DueDate < dto.Date)
            return (false, "DueDate must be equal or after Date.", null);

        var medicamentIds = dto.Medicaments.Select(m => m.IdMedicament).ToList();
        var existingMedicaments = await _context.Medicaments
            .Where(m => medicamentIds.Contains(m.IdMedicament))
            .Select(m => m.IdMedicament)
            .ToListAsync();

        if (existingMedicaments.Count != medicamentIds.Count)
            return (false, "One or more medicaments do not exist.", null);

        var patient = await _context.Patients
            .FirstOrDefaultAsync(p =>
                p.FirstName == dto.Patient.FirstName &&
                p.LastName == dto.Patient.LastName &&
                p.Birthdate == dto.Patient.Birthdate);

        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = dto.Patient.FirstName,
                LastName = dto.Patient.LastName,
                Birthdate = dto.Patient.Birthdate
            };
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        var doctor = await _context.Doctors.FindAsync(dto.DoctorId);
        if (doctor == null)
            return (false, "Doctor not found.", null);

        var prescription = new Prescription
        {
            Date = dto.Date,
            DueDate = dto.DueDate,
            IdPatient = patient.IdPatient,
            IdDoctor = doctor.IdDoctor
        };

        await _context.Prescriptions.AddAsync(prescription);
        await _context.SaveChangesAsync();

        foreach (var med in dto.Medicaments)
        {
            _context.PrescriptionMedicaments.Add(new PrescriptionMedicament
            {
                IdPrescription = prescription.IdPrescription,
                IdMedicament = med.IdMedicament,
                Dose = med.Dose,
                Description = med.Description
            });
        }

        await _context.SaveChangesAsync();
        return (true, null, prescription.IdPrescription);
    }
}
