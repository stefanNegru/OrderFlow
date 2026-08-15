namespace OrderFlow.Application.Customers.Exceptions;

public sealed class CustomerNotFoundException(Guid customerId) : Exception($"Customer with ID '{customerId}' was not found.");
