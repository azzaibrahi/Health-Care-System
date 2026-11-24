using Health_Care_System.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Health_Care_System.Middlewares;

/// <summary>
/// Catches anything that escapes a controller and writes the shared
/// <see cref="ProblemDetails"/> envelope instead of the default developer page.
/// </summary>
public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger,
    IHostEnvironment environment)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            if (context.Response.HasStarted)
            {
                logger.LogError(
                    exception,
                    "Request {TraceId} failed after the response started; it cannot be replaced",
                    context.TraceIdentifier);

                throw;
            }

            logger.LogError(
                exception,
                "Unhandled exception while processing request {TraceId}",
                context.TraceIdentifier);

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.Headers[RequestIdHeaderMiddleware.HeaderName] = context.TraceIdentifier;

            var problem = ExceptionProblemDetailsFactory.Create(exception, context, environment);

            await context.Response.WriteAsJsonAsync(
                problem,
                options: null,
                contentType: "application/problem+json",
                cancellationToken: context.RequestAborted);
        }
    }
}
