using GlobalExceptionHandlerApi.SharedKernel.Results;

namespace GlobalExceptionHandlerApi.Domain;
public class CustomerServices : ICustomerServices
{
    private static List<Customer> _customers = [];

    public async Task<Result> CreateCustomer(Customer customer)
    {
        return await Task.Run(() =>
        {
            if (customer.Id <= 0 || string.IsNullOrWhiteSpace(customer.Name) || string.IsNullOrWhiteSpace(customer.Email))
            {
                return Result.Failure(CustomerErrors.InvalidCustomerData);
            }

            _customers.Add(customer);
            return Result.Success();
        });
    }

    public async Task<IEnumerable<Customer>> GetCustomers()
    {
        return await Task.Run(() => _customers.AsEnumerable());
    }
}
