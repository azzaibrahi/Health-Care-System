using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Health_Care_System.Filters;

/// <summary>
/// Measures the time spent inside an MVC action and writes one structured line.
/// </summary>
public class ActionTimingFilter(ILogger<ActionTimingFilter> logger) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await next();
        }
        finally
        {
            stopwatch.Stop();

            logger.LogInformation(
                "Action {ActionName} took {ElapsedMilliseconds} ms",
                context.ActionDescriptor.DisplayName,
                stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}
