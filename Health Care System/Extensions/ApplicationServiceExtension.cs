using Health_Care_System.Data;
using Health_Care_System.Data.Interceptors;
using Health_Care_System.Entities;
using Health_Care_System.Filters;
using Health_Care_System.Helper;
using Health_Care_System.Middlewares;
using Health_Care_System.Repositories;
using Health_Care_System.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        #region Session 01

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

        #endregion

        #region Session 02

        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<PatientProfile>();
            cfg.AddProfile<DoctorProfile>();
            cfg.AddProfile<AppointmentProfile>();
        });

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();

        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IAppointmentService, AppointmentService>();

        #endregion

        #region Session 03

        // IMiddleware types are activated through DI, so they must be registered.
        services.AddScoped<RequestIdHeaderMiddleware>();

        #endregion

        #region Session 04

        // ServiceFilter attributes resolve these filters from DI.
        services.AddScoped<ActionExecutionLoggingFilter>();
        services.AddScoped<ActionTimingFilter>();
        services.AddScoped<ExceptionHandlingFilter>();

        services.AddDbContext<AuthenticationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("AuthenticationConnection")));

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AuthenticationDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = JwtSigningKey.Create(configuration)
            };
        });

        services.AddScoped<IJwtService, JwtService>();

        #endregion

        return services;
    }
}
