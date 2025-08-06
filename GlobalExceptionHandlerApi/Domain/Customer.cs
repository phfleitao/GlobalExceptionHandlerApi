namespace GlobalExceptionHandlerApi.Domain;
public class Customer
{
    public Customer(int id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
}
