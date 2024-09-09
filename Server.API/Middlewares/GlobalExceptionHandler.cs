using Microsoft.AspNetCore.Diagnostics;
using Server.BLL.Exceptions;
using Server.BLL.Models;

namespace Server.API.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An error occurred while processing your request");

        (int statusCode, string title) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
            DbOperationException => (StatusCodes.Status503ServiceUnavailable, "Service Unavailable"),
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error"),
        };

        var errorResponse = new ErrorResponse
        {
            StatusCode = statusCode,
            Title = title,
            Message = exception.Message,
        };

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsync(errorResponse.ToString(), cancellationToken);

        return true;
    }
}
