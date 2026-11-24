using Microsoft.AspNetCore.Identity;

namespace Health_Care_System.Entities;

/// <summary>
/// Identity account used to authenticate API callers.
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>Display name of the account holder.</summary>
    public string FullName { get; set; } = string.Empty;
}
