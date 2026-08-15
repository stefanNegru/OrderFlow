using System;
using System.Collections.Generic;
using System.Text;

namespace OrderFlow.Application.Customers.Services;

public interface ICustomerService
{
    Task<IReadOnlyList<CustomerResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CustomerResponse?> GetByIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<CustomerResponse> CreateAsync (CreateCustomerRequest request, CancellationToken cancellationToken = default);
    Task<CustomerResponse> UpdateAsync (Guid customerId, UpdateCustomerRequest request, CancellationToken cancellationToken = default);
}
