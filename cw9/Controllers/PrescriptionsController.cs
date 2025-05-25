using Microsoft.AspNetCore.Mvc;
using cw9.DTOs;
using cw9.Services;

namespace cw9.Controllers;

[ApiController]
[Route("api/prescriptions")]
public class PrescriptionsController : ControllerBase
{
    private readonly IPrescriptionService _service;

    public PrescriptionsController(IPrescriptionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromBody] AddPrescriptionRequestDTO request)
    {
        var result = await _service.AddPrescriptionAsync(request);

        if (!result.IsSuccess)
        {
            return BadRequest(new { error = result.ErrorMessage });
        }

        return CreatedAtAction(nameof(AddPrescription), new { id = result.PrescriptionId }, new { id = result.PrescriptionId });
    }
}