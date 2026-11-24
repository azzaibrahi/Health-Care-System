using Microsoft.AspNetCore.Mvc.Filters;

namespace Health_Care_System.Filters;

/// <summary>
/// Logs when an MVC action starts and finishes. Only the action name is logged;
/// arguments and bodies can carry personal data and are never written.
/// </summary>
public class ActionExecutionLoggingFilter(ILogger<ActionExecutionLoggingFilter> logger) : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        logger.LogInformation(
            "Starting action {ActionName}",
            context.ActionDescriptor.DisplayName);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var actionName = context.ActionDescriptor.DisplayName;

        if (context.Exception is not null && !context.ExceptionHandled)
        {
            logger.LogError(
                context.Exception,
                "Action {ActionName} failed.",
                actionName);

            return;
        }

        logger.LogInformation(
            "Finishing action {ActionName}",
            actionName);
    }
}
