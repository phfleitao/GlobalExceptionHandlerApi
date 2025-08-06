namespace GlobalExceptionHandlerApi.Domain;
public class CustomerServices : ICustomerServices
{
    public IEnumerable<Customer> GetCustomers()
    {
        return Enumerable.Range(1, 5)
            .Select(index => new Customer(
                index,
                $"Customer {index}",
                $"customer{index}@email.com")
            ).ToArray();
    }
}
