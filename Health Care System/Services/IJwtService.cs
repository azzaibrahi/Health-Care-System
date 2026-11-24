using Health_Care_System.Entities;

namespace Health_Care_System.Services;

/// <summary>Creates a signed JWT for an authenticated Identity user.</summary>
public interface IJwtService
{
    /// <summary>Returns a signed bearer token for <paramref name="user"/>.</summary>
    string GenerateToken(ApplicationUser user);
}
