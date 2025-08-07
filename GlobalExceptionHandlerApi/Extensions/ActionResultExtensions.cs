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
            Title = "Business error",
            Detail = error.Description,
            Status = GetErrorStatusCode(error.Type),
            Type = GetErrorStatusCodeRFC(error.Type),
            Extensions =
            {
                ["errors"] = new[] { error.Description }
            }
        };
        
        return new ObjectResult(problemDetails) 
        { 
            StatusCode = GetErrorStatusCode(error.Type) 
        };
    }

    private static int GetErrorStatusCode(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private static string GetErrorStatusCodeRFC(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Failure => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1",
            ErrorType.Validation => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
            ErrorType.NotFound => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
            ErrorType.Conflict => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.10",
            _ => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1"
        };
    }
}
