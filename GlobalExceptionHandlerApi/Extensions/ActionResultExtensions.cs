using GlobalExceptionHandlerApi.SharedKernel.Errors;
using GlobalExceptionHandlerApi.SharedKernel.Results;
using Microsoft.AspNetCore.Mvc;

namespace GlobalExceptionHandlerApi.Extensions;
public static class ActionResultExtensions
{
    public static IActionResult ToResultProblemDetails(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Cannot convert a successful result to ProblemDetails.");
        }

        var error = result.Error;

        var problemDetails = new ProblemDetails
        {
            Title = GetTitle(error.Type),
            Status = GetStatusCode(error.Type),
            Type = GetStatusCodeRFC(error.Type),
            Extensions = new Dictionary<string, object?>
            {
                { "errors", new[] { error } }
            }
        };
        
        return new ObjectResult(problemDetails) 
        { 
            StatusCode = GetStatusCode(error.Type) 
        };
    }

    private static string GetTitle(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => "Bad Request",
            ErrorType.NotFound => "Not Found",
            ErrorType.Conflict => "Conflict",
            _ => "Server Failure"
        };

    private static int GetStatusCode(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

    private static string GetStatusCodeRFC(ErrorType errorType) => 
        errorType switch
        {
            ErrorType.Failure => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1",
            ErrorType.Validation => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
            ErrorType.NotFound => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
            ErrorType.Conflict => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.10",
            _ => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1"
        };
}
