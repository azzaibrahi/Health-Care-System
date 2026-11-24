using Health_Care_System.Data;
using Health_Care_System.Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

namespace Health_Care_System.Extensions;

/// <summary>
/// Registers the services used by the Health Care API.
/// </summary>
public static class ApplicationServiceExtension
{
    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter a valid JWT token."
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
        });

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultSqlConnection"));

            options.AddInterceptors(
                new SoftDeleteInterceptor(),
                new CreatedAtInterceptor(),
                new ModifiedAtInterceptor());
        });

        return services;
    }
}
