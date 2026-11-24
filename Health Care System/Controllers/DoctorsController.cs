using Health_Care_System.Dtos.DoctorDtos;
using Health_Care_System.Results;
using Health_Care_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Health_Care_System.Controllers;

/// <summary>
/// Read-only doctor endpoints.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class DoctorsController(IDoctorService service) : ControllerBase
{
    /// <summary>Returns every doctor.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(GenericResult<IEnumerable<GetDoctorResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await service.GetAllAsync();

        return Ok(result);
    }

    /// <summary>Returns one doctor by id.</summary>
    /// <param name="id">Doctor id.</param>
    /// <response code="200">The doctor was found.</response>
    /// <response code="404">No doctor has this id.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GenericResult<GetDoctorResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GenericResult<GetDoctorResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await service.GetByIdAsync(id);

        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
