using OrderFlow.Application.Customers.Repositories;
using OrderFlow.Domain.Customers;
using OrderFlow.Application.Customers.Exceptions;

namespace OrderFlow.Application.Customers.Services;

public sealed class CustomerService(ICustomerRepository customerRepository) : ICustomerService
{
    public async Task<IReadOnlyList<CustomerResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var customers = await customerRepository.GetAllAsync(cancellationToken);

        return [.. customers.Select(Map)];
    }

    public async Task<CustomerResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetByIdAsync(id, cancellationToken);

        //return customer is null ? null : Map(customer);
        if (customer == null) 
            throw new CustomerNotFoundException(id);

        return Map(customer);
    }

    public async Task<CustomerResponse> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var emailExists = await customerRepository.ExistsByEmailAsync(request.Email, null, cancellationToken);

        if (emailExists)
            throw new CustomerEmailAlreadyExistsException(request.Email);

        var customer = new Customer(
            request.Name,
            request.Email,
            request.Phone);

        await customerRepository.AddAsync(customer, cancellationToken);

        await customerRepository.SaveChangesAsync(cancellationToken);

        return Map(customer);
    }

    public async Task<CustomerResponse> UpdateAsync(
        Guid customerId,
        UpdateCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        var emailExists = await customerRepository.ExistsByEmailAsync(request.Email, customerId, cancellationToken);

        if (emailExists)
            throw new CustomerEmailAlreadyExistsException(request.Email);

        var customer = await customerRepository.GetTrackedByIdAsync(customerId, cancellationToken);

        if (customer is null)
            throw new CustomerNotFoundException(customerId);

        customer.Update(
            request.Name,
            request.Email,
            request.Phone);
        await customerRepository.SaveChangesAsync(cancellationToken);
        return Map(customer);
    }

    private static CustomerResponse Map(Customer customer)
    {
        return new CustomerResponse(
            customer.Id,
            customer.Name,
            customer.Email,
            customer.Phone,
            customer.CreatedAtUtc);
    }
}
