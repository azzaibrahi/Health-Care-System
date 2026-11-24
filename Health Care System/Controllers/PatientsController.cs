using Health_Care_System.Dtos.PatientDtos;
using Health_Care_System.Results;
using Health_Care_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Health_Care_System.Controllers;

/// <summary>
/// Read-only patient endpoints. They exist to show the full chain
/// controller → service → repository → AutoMapper → result.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PatientsController(IPatientService service) : ControllerBase
{
    /// <summary>Returns every patient.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(GenericResult<IEnumerable<GetPatientResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await service.GetAllAsync();

        return Ok(result);
    }

    /// <summary>Returns one patient by id.</summary>
    /// <param name="id">Patient id.</param>
    /// <response code="200">The patient was found.</response>
    /// <response code="404">No patient has this id.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GenericResult<GetPatientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GenericResult<GetPatientResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await service.GetByIdAsync(id);

        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
