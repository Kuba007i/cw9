using cw9.DTOs;

namespace cw9.Services;

public interface IPatientService
{
    Task<PatientDetailsResponseDTO?> GetPatientAsync(int patientId);
}