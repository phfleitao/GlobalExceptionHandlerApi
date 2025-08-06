using Microsoft.AspNetCore.Mvc;

namespace GlobalExceptionHandlerApi.Exceptions;
internal sealed class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        RequestDelegate next,
        ILogger<GlobalExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {           
            await HandleExceptionAsync(context, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception occurred while processing the request at {Time}", DateTime.UtcNow);

        context.Response.StatusCode = exception switch 
        {             
            ArgumentException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Type = exception.GetType().Name,
            Title = "An error occurred while processing your request.",
            Detail = exception.Message
        });
    }
}
