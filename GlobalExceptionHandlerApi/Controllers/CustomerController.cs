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
    public async Task<IActionResult> CreateCustomer([FromBody]Customer customer)
    {
        _logger.LogInformation("Creating customer at {Time}", DateTime.UtcNow);

        if (ModelState.IsValid is false)
        {
            _logger.LogWarning("Invalid model state for customer creation at {Time}", DateTime.UtcNow);

            return BadRequest(ModelState);
        }

        ValidateRequest(customer);
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
