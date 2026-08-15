namespace OrderFlow.Application.Customers.Exceptions;

public sealed class CustomerEmailAlreadyExistsException(string email) : Exception($"Customer with email {email} already exists.");
