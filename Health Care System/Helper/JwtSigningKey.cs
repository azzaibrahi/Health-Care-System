using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Health_Care_System.Helper;

/// <summary>
/// Reads the JWT signing key from configuration. The value is Base64 so a full
/// 48-byte CSPRNG key can be stored in user-secrets without an invalid character.
/// </summary>
internal static class JwtSigningKey
{
    public static SymmetricSecurityKey Create(IConfiguration configuration)
    {
        var value = configuration["Jwt:Key"];

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException(
                "Jwt:Key is missing. Store it with user-secrets or an environment variable.");
        }

        byte[] keyBytes;

        try
        {
            keyBytes = Convert.FromBase64String(value);
        }
        catch (FormatException exception)
        {
            throw new InvalidOperationException(
                "Jwt:Key must be a Base64-encoded value.",
                exception);
        }

        if (keyBytes.Length < 32)
        {
            throw new InvalidOperationException(
                "Jwt:Key must decode to at least 32 bytes.");
        }

        return new SymmetricSecurityKey(keyBytes);
    }
}
