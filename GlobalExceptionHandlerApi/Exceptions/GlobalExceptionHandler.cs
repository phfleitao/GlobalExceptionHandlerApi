using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GlobalExceptionHandlerApi.Exceptions;
internal sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred while processing the request at {Time}", DateTime.UtcNow);
        httpContext.Response.StatusCode = ExtractStatusCode(exception);

        var context = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = ExtractProblemDetails(exception)
        };
        
        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
        context.ProblemDetails.Extensions.TryAdd("errors", new[] { exception.Message }); //Replace with list errors if needed

        return await problemDetailsService.TryWriteAsync(context);
    }

    private int ExtractStatusCode(Exception exception)
    {
        return exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private ProblemDetails ExtractProblemDetails(Exception exception)
    {
        return exception switch
        {
            ArgumentException => BadRequestProblemDetails(exception),
            _ => InternalServerProblemDetails(exception)
        };        
    }

    private ProblemDetails BadRequestProblemDetails(Exception exception)
    {
        return new ProblemDetails
        {
            Detail = "One or more validation errors occurred",
            Status = ExtractStatusCode(exception)
        };
    }

    private ProblemDetails InternalServerProblemDetails(Exception exception)
    {
        return new ProblemDetails
        {
            Detail = "One or more errors occurred",
            Status = ExtractStatusCode(exception)
        };
    }
}
