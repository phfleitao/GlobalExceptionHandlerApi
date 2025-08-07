using GlobalExceptionHandlerApi.SharedKernel.Errors;

namespace GlobalExceptionHandlerApi.Domain
{
    public static class CustomerErrors
    {
        public static readonly Error CustomerGenericFailure = Error.Failure("Customer.GenericFailure", "An internal error occured while processing customer data.");
        public static readonly Error CustomerNotFound = Error.NotFound("Customer.NotFound", "The specified customer was not found.");
        public static readonly Error CustomerAlreadyExists = Error.Conflict("Customer.AlreadyExists", "A customer with the same identifier already exists.");
        public static readonly Error InvalidCustomerData = Error.Validation("Customer.InvalidData", "The provided customer data is invalid.");
    }
}
