using Health_Care_System.Dtos.AppointmentDtos;
using Health_Care_System.Results;
using Health_Care_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Health_Care_System.Controllers;

/// <summary>
/// Read-only appointment endpoints. An appointment is addressed by its
/// composite key, so the date travels as a query string.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AppointmentsController(IAppointmentService service) : ControllerBase
{
    /// <summary>Returns every appointment.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(GenericResult<IEnumerable<GetAppointmentResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await service.GetAllAsync();

        return Ok(result);
    }

    /// <summary>Returns one appointment by its composite key.</summary>
    /// <param name="patientId">Patient id.</param>
    /// <param name="doctorId">Doctor id.</param>
    /// <param name="appointmentDate">Appointment date and time.</param>
    /// <response code="200">The appointment was found.</response>
    /// <response code="404">No appointment matches the key.</response>
    [HttpGet("{patientId:int}/{doctorId:int}")]
    [ProducesResponseType(typeof(GenericResult<GetAppointmentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GenericResult<GetAppointmentResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        int patientId,
        int doctorId,
        [FromQuery] DateTime appointmentDate)
    {
        var result = await service.GetByIdAsync(patientId, doctorId, appointmentDate);

        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
