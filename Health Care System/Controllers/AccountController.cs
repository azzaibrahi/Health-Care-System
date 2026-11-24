using Health_Care_System.Dtos.AccountDtos;
using Health_Care_System.Entities;
using Health_Care_System.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Health_Care_System.Controllers;

/// <summary>
/// Anonymous account endpoints. Registration creates an Identity user and login
/// returns the signed JWT used by the protected health-care endpoints.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AccountController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IJwtService jwtService) : ControllerBase
{
    /// <summary>Creates a new account.</summary>
    /// <param name="request">Email and password.</param>
    /// <response code="201">The account was created.</response>
    /// <response code="400">Identity refused the email or password.</response>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(UserRegisterRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.Email
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Created();
    }

    /// <summary>Returns a signed bearer token for valid credentials.</summary>
    /// <param name="request">Registered email and password.</param>
    /// <response code="200">The raw JWT token.</response>
    /// <response code="401">Unknown email or wrong password.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(UserLoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return Unauthorized();
        }

        var result = await signInManager.CheckPasswordSignInAsync(
            user,
            request.Password,
            lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            return Unauthorized();
        }

        return Ok(jwtService.GenerateToken(user));
    }
}
