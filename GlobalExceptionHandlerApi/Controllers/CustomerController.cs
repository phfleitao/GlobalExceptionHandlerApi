using GlobalExceptionHandlerApi.Domain;
using Microsoft.AspNetCore.Mvc;

namespace GlobalExceptionHandlerApi.Presentation.Controllers;

[ApiController]
[Route("/customer")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerServices _customerServices;
    private readonly ILogger _logger;
    public CustomerController(ILogger<CustomerController> logger, ICustomerServices customerServices)
    {
        _logger = logger;
        _customerServices = customerServices;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
    {
        _logger.LogInformation("Creating customer at {Time}", DateTime.UtcNow);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for customer creation at {Time}", DateTime.UtcNow);

            var problemDetails = new ProblemDetails
            {
                Title = "Invalid model state",
                Detail = "The request contains invalid data. Please check the provided information.",
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };
            return new ObjectResult(problemDetails) { StatusCode = StatusCodes.Status400BadRequest };
        }

        if (customer.Id <= 0)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Invalid request",
                Detail = "Invalid Id",
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Extensions = {
                    ["requestId"] = HttpContext.TraceIdentifier,
                    ["errors"] = new[] { "Id must be greater than zero." }
                }
            };
            return new ObjectResult(problemDetails) { StatusCode = StatusCodes.Status400BadRequest };
        }

        if (string.IsNullOrWhiteSpace(customer.Name))
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Invalid request",
                Detail = "Invalid Name",
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };
            return new ObjectResult(problemDetails) { StatusCode = StatusCodes.Status400BadRequest };
        }

        if (string.IsNullOrWhiteSpace(customer.Email))
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Invalid request",
                Detail = "Invalid Email",
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };
            return new ObjectResult(problemDetails) { StatusCode = StatusCodes.Status400BadRequest };
        }

        await _customerServices.CreateCustomer(customer);

        return Created(string.Empty, customer);
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        _logger.LogInformation("Fetching customers at {Time}", DateTime.UtcNow);

        return Ok(await _customerServices.GetCustomers());
    }

    private void ValidateRequest(Customer customer)
    {
        if (customer.Id <= 0)
            throw new ArgumentException("Invalid Id");

        if(string.IsNullOrWhiteSpace(customer.Name))
            throw new ArgumentException("Invalid Name");

        if (string.IsNullOrWhiteSpace(customer.Email))
            throw new ArgumentException("Invalid Email");
    }
}
