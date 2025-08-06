using GlobalExceptionHandlerApi.Domain;
using Microsoft.AspNetCore.Mvc;

namespace GlobalExceptionHandlerApi.Presentation.Controllers;

[ApiController]
[Route("/")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerServices _customerServices;
    private readonly ILogger _logger;
    public CustomerController(ILogger<CustomerController> logger, ICustomerServices customerServices)
    {
        _logger = logger;
        _customerServices = customerServices;
    }

    [HttpGet("customers")]
    public IEnumerable<Customer> GetCustomers()
    {
        _logger.LogInformation("Fetching customers at {Time}", DateTime.UtcNow);
        return _customerServices.GetCustomers();
    }
}
