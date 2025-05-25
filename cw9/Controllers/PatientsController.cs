using Microsoft.AspNetCore.Mvc;
using cw9.Services;

namespace cw9.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _service;

    public PatientsController(IPatientService service)
    {
        _service = service;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPatient(int id)
    {
        var result = await _service.GetPatientAsync(id);

        if (result is null)
            return NotFound(new { error = "Patient not found" });

        return Ok(result);
    }
}