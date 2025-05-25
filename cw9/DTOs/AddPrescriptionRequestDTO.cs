namespace cw9.DTOs;

public class AddPrescriptionRequestDTO
{
    public PatientDTO Patient { get; set; } = null!;
    public int DoctorId { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<AddMedicamentDTO> Medicaments { get; set; } = new();
}

public class PatientDTO
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime Birthdate { get; set; }
}

public class AddMedicamentDTO
{
    public int IdMedicament { get; set; }
    public int Dose { get; set; }
    public string Description { get; set; } = null!;
}