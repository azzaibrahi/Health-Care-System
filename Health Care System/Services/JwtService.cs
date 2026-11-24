using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Health_Care_System.Entities;
using Health_Care_System.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Health_Care_System.Services;

/// <inheritdoc />
public class JwtService(IConfiguration configuration) : IJwtService
{
    private readonly IConfiguration _configuration = configuration;

    public string GenerateToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!)
        };

        var credentials = new SigningCredentials(
            JwtSigningKey.Create(_configuration),
            SecurityAlgorithms.HmacSha256);

        var durationInMinutes = _configuration.GetValue<double>("Jwt:DurationInMinutes");

        if (durationInMinutes <= 0)
        {
            throw new InvalidOperationException("Jwt:DurationInMinutes must be positive.");
        }

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(durationInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
