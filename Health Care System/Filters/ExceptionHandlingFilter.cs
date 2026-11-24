using Health_Care_System.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Health_Care_System.Filters;

/// <summary>
/// Converts an exception thrown by an MVC action into the shared
/// <see cref="ProblemDetails"/> envelope.
/// </summary>
public class ExceptionHandlingFilter(
    ILogger<ExceptionHandlingFilter> logger,
    IHostEnvironment environment) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.ExceptionHandled)
        {
            return;
        }

        logger.LogError(
            context.Exception,
            "Unhandled exception in action {ActionName} for request {TraceId}",
            context.ActionDescriptor.DisplayName,
            context.HttpContext.TraceIdentifier);

        var problem = ExceptionProblemDetailsFactory.Create(
            context.Exception,
            context.HttpContext,
            environment);

        var result = new ObjectResult(problem)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        result.ContentTypes.Add("application/problem+json");

        context.Result = result;
        context.ExceptionHandled = true;
    }
}
