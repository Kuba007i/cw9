using Microsoft.EntityFrameworkCore;
using cw9.Data;
using cw9.DTOs;

namespace cw9.Services;

public class PatientService : IPatientService
{
    private readonly AppDbContext _context;

    public PatientService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PatientDetailsResponseDTO?> GetPatientAsync(int patientId)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.Doctor)
            .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.PrescriptionMedicaments)
                    .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == patientId);

        if (patient == null)
            return null;

        return new PatientDetailsResponseDTO
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.Birthdate,
            Prescriptions = patient.Prescriptions
                .OrderBy(pr => pr.DueDate)
                .Select(pr => new PrescriptionResponseDTO
                {
                    IdPrescription = pr.IdPrescription,
                    Date = pr.Date,
                    DueDate = pr.DueDate,
                    Doctor = new DoctorDTO
                    {
                        IdDoctor = pr.Doctor.IdDoctor,
                        FirstName = pr.Doctor.FirstName,
                        LastName = pr.Doctor.LastName
                    },
                    Medicaments = pr.PrescriptionMedicaments.Select(pm => new MedicamentResponseDTO
                    {
                        IdMedicament = pm.Medicament.IdMedicament,
                        Name = pm.Medicament.Name,
                        Description = pm.Description,
                        Dose = pm.Dose
                    }).ToList()
                }).ToList()
        };
    }
}
