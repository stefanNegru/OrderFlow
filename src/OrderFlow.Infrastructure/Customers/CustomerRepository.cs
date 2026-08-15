using OrderFlow.Infrastructure.Persistence;
using OrderFlow.Application.Customers.Repositories;
using OrderFlow.Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace OrderFlow.Infrastructure.Customers;

public sealed class CustomerRepository(OrderFlowDbContext dbContext) : ICustomerRepository
{
    public async Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Customers.AsNoTracking().ToListAsync(cancellationToken);
    }
    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
    public Task<Customer?> GetTrackedByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
    public async Task<bool> ExistsByEmailAsync(string email, Guid? excludeCustomerId = null, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        return await dbContext.Customers.AnyAsync(
            c => c.Email == normalizedEmail &&
                (!excludeCustomerId.HasValue || c.Id != excludeCustomerId.Value), cancellationToken);
    }
    public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await dbContext.Customers.AddAsync(customer, cancellationToken);
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
