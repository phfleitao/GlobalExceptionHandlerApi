using GlobalExceptionHandlerApi.Domain;
using GlobalExceptionHandlerApi.Extensions;
using GlobalExceptionHandlerApi.SharedKernel.Results;
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
            return Result.Failure(CustomerErrors.InvalidCustomerData).ToResultProblemDetails();
        }

        var result = await _customerServices.CreateCustomer(customer);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetCustomers), new { id = customer.Id }, customer)
            : result.ToResultProblemDetails();
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        _logger.LogInformation("Fetching customers at {Time}", DateTime.UtcNow);

        return Ok(await _customerServices.GetCustomers());
    }
}
