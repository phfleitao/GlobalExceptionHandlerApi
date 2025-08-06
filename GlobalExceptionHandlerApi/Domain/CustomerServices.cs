namespace GlobalExceptionHandlerApi.Domain;
public class CustomerServices : ICustomerServices
{
    private static List<Customer> _customers = [];

    public async Task CreateCustomer(Customer customer)
    {
        await Task.Run(() => _customers.Add(customer));
    }

    public async Task<IEnumerable<Customer>> GetCustomers()
    {
        return await Task.Run(() => _customers.AsEnumerable());
    }
}
