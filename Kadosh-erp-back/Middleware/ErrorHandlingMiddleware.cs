using Domain.Exceptions;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UserException ex)
        {
            await HandleUserExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleSystemExceptionAsync(context, ex);
        }
    }

    private Task HandleUserExceptionAsync(HttpContext context, UserException ex)
    {
        var response = new
        {
            statusCode = (int)HttpStatusCode.BadRequest,
            message = ex.Message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private Task HandleSystemExceptionAsync(HttpContext context, Exception ex)
    {
        var traceId = Guid.NewGuid().ToString();

        _logger.LogError(ex, "Unhandled exception [{TraceId}]: {Message}", traceId, ex.Message);

        var response = new
        {
            statusCode = (int)HttpStatusCode.InternalServerError,
            message = "Error de sistema. Contacte al administrador.",
            traceId
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}