using Health_Care_System.Extensions;
using Health_Care_System.Middlewares;

namespace Health_Care_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddServices(builder.Configuration);

            var app = builder.Build();

            // Request id first, so even an error response carries the header.
            app.UseMiddleware<RequestIdHeaderMiddleware>();

            // Timing wraps the rest, so a failed request is measured too.
            app.UseMiddleware<RequestTimingMiddleware>();

            // Exception handler converts unhandled failures to ProblemDetails.
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseMiddleware<RateLimitingMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                // Swagger stores the raw token; send it as a bearer credential.
                options.UseRequestInterceptor("""
                    function (request) {
                        const authorization = request.headers.Authorization || request.headers.authorization;

                        if (authorization && !/^Bearer\s/i.test(authorization)) {
                            request.headers.Authorization = `Bearer ${authorization}`;
                            delete request.headers.authorization;
                        }

                        return request;
                    }
                    """);
            });

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
