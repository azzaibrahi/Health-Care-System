using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;

namespace Health_Care_System.Middlewares;

/// <summary>
/// Fixed-window limiter: at most 10 requests per client IP every 10 seconds.
/// A 429 answer carries <c>Retry-After</c> and the same request id as every
/// other response.
/// </summary>
public class RateLimitingMiddleware(RequestDelegate next)
{
    private const int MaxRequests = 10;
    private static readonly TimeSpan Window = TimeSpan.FromSeconds(10);
    private static readonly ConcurrentDictionary<string, WindowCounter> Counters = new();

    public async Task InvokeAsync(HttpContext context)
    {
        var client = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var now = DateTime.UtcNow;

        var counter = Counters.AddOrUpdate(
            client,
            _ => new WindowCounter(1, now),
            (_, current) => now - current.WindowStart >= Window
                ? new WindowCounter(1, now)
                : new WindowCounter(current.Count + 1, current.WindowStart));

        if (counter.Count > MaxRequests)
        {
            var retryAfter = (int)Math.Ceiling((Window - (now - counter.WindowStart)).TotalSeconds);

            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.Response.Headers.RetryAfter = retryAfter.ToString();
            context.Response.Headers[RequestIdHeaderMiddleware.HeaderName] = context.TraceIdentifier;

            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status429TooManyRequests,
                Title = "Too many requests.",
                Detail = "Wait a few seconds before trying again.",
                Instance = context.Request.Path
            };

            await context.Response.WriteAsJsonAsync(
                problem,
                options: null,
                contentType: "application/problem+json",
                cancellationToken: context.RequestAborted);

            return;
        }

        await next(context);
    }

    private sealed record WindowCounter(int Count, DateTime WindowStart);
}
