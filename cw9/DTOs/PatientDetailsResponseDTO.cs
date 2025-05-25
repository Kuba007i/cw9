namespace cw9.DTOs;

public class PatientDetailsResponseDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime Birthdate { get; set; }
    public List<PrescriptionResponseDTO> Prescriptions { get; set; } = new();
}

public class PrescriptionResponseDTO
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public DoctorDTO Doctor { get; set; } = null!;
    public List<MedicamentResponseDTO> Medicaments { get; set; } = new();
}

public class MedicamentResponseDTO
{
    public int IdMedicament { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Dose { get; set; }
}

public class DoctorDTO
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}