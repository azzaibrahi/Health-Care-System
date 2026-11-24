namespace Health_Care_System.Middlewares;

/// <summary>
/// Stamps every response with the request identifier that ASP.NET Core
/// already generated, so a client can quote it when reporting a failure.
/// </summary>
/// <remarks>
/// This middleware is registered as a scoped service in
/// <c>ApplicationServiceExtension</c> because it implements <see cref="IMiddleware"/>.
/// </remarks>
public class RequestIdHeaderMiddleware(ILogger<RequestIdHeaderMiddleware> logger) : IMiddleware
{
    public const string HeaderName = "X-Request-Id";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var requestId = context.TraceIdentifier;

        context.Response.Headers[HeaderName] = requestId;

        logger.LogDebug(
            "Assigned request ID {RequestId} to {Method} {Path}",
            requestId,
            context.Request.Method,
            context.Request.Path);

        await next(context);
    }
}
