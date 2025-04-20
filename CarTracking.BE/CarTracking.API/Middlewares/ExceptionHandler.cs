using CarTracking.BE.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CarTracking.API.Middlewares;

public class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Title = "Server error",
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception.Message
        };
        httpContext.Response.ContentType = "application/json";
        switch (exception)
        {
            case NotFoundException notFoundExcepiton:
                httpContext.Response.StatusCode = 404;
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Title = notFoundExcepiton.Code;
                break;
            default:
                httpContext.Response.StatusCode = 500;
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Title = exception.Message;
                break;
        }
        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}