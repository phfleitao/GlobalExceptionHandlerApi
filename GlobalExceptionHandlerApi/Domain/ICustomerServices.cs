using GlobalExceptionHandlerApi.SharedKernel.Results;

namespace GlobalExceptionHandlerApi.Domain;
public interface ICustomerServices
{
    Task<Result> CreateCustomer(Customer customer);
    Task<IEnumerable<Customer>> GetCustomers();
}
