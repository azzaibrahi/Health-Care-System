using Health_Care_System.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Health_Care_System.Data;

/// <summary>
/// Identity store. It lives in a separate database from the health-care data.
/// </summary>
public class AuthenticationDbContext : IdentityDbContext<ApplicationUser>
{
    public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options)
        : base(options)
    {
    }
}
