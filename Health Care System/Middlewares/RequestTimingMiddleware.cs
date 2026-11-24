using System.Diagnostics;

namespace Health_Care_System.Middlewares;

/// <summary>
/// Measures how long every request takes and writes one structured log line
/// with the method, the path, the status code and the elapsed milliseconds.
/// </summary>
/// <remarks>
/// Only identifiers are logged. Bodies, query strings and headers are never
/// written, because they can carry medical or personal data.
/// </remarks>
public class RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        Exception? failure = null;

        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            failure = exception;
            throw;
        }
        finally
        {
            stopwatch.Stop();

            if (failure is null)
            {
                logger.LogInformation(
                    "HTTP {Method} {Path} returned {StatusCode} in {ElapsedMilliseconds} ms",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopwatch.Elapsed.TotalMilliseconds);
            }
            else
            {
                logger.LogError(
                    failure,
                    "HTTP {Method} {Path} failed after {ElapsedMilliseconds} ms",
                    context.Request.Method,
                    context.Request.Path,
                    stopwatch.Elapsed.TotalMilliseconds);
            }
        }
    }
}
