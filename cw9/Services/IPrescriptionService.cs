using cw9.DTOs;

namespace cw9.Services;

public interface IPrescriptionService
{
    Task<(bool IsSuccess, string? ErrorMessage, int? PrescriptionId)> AddPrescriptionAsync(AddPrescriptionRequestDTO dto);
}