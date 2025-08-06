namespace GlobalExceptionHandlerApi.Domain;
public interface ICustomerServices
{
    Task CreateCustomer(Customer customer);
    Task<IEnumerable<Customer>> GetCustomers();
}
