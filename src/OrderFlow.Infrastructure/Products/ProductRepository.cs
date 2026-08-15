using Microsoft.EntityFrameworkCore;
using OrderFlow.Application.Common;
using OrderFlow.Application.Products.Dtos;
using OrderFlow.Application.Products.Repositories;
using OrderFlow.Domain.Products;
using OrderFlow.Infrastructure.Persistence;

namespace OrderFlow.Infrastructure.Products;

public sealed class ProductRepository(OrderFlowDbContext context) : IProductRepository
{
    public async Task<PagedResult<Product>> GetAllAsync(ProductQueryParameters parameters, CancellationToken cancellationToken = default)
    {
        IQueryable<Product> query = context.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(parameters.Search))
        {
            var search = parameters.Search.Trim();

            query = query.Where(product =>
               product.Name.Contains(search) ||
                product.Sku.Contains(search));
        }

        if (parameters.IsActive.HasValue)
        {
            query = query.Where(product =>
                product.IsActive == parameters.IsActive.Value);
        }

        query = parameters.SortBy.ToLowerInvariant() switch
        {
            "price" => parameters.SortDirection.ToLowerInvariant() == "desc"
                ? query.OrderByDescending(product => product.Price)
                : query.OrderBy(product => product.Price),

            "sku" => parameters.SortDirection.ToLowerInvariant() == "desc"
                ? query.OrderByDescending(product => product.Sku)
                : query.OrderBy(product => product.Sku),

            _ => parameters.SortDirection.ToLowerInvariant() == "desc"
                ? query.OrderByDescending(product => product.Name)
                : query.OrderBy(product => product.Name)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var products = await query
            .Skip((parameters.Page - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Product>(
            products,
            parameters.Page,
            parameters.PageSize,
            totalCount);
    }
    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellation = default)
    {
        return await context.Products.FindAsync(new object[] { id }, cancellation);
    }
    public async Task<bool> ExistsBySkuAsync(string sku, CancellationToken cancellation = default)
    {
        return await context.Products.AnyAsync(p => p.Sku == sku, cancellation);
    }
    public async Task AddAsync(Product product, CancellationToken cancellation = default)
    {
        await context.Products.AddAsync(product, cancellation);
    }
    public async Task SaveChangesAsync(CancellationToken cancellation = default)
    {
        await context.SaveChangesAsync(cancellation);
    }
    public async Task<Product?> GetTrackedByIdAsync(Guid id, CancellationToken cancellation = default)
    {
        return await context.Products.FirstOrDefaultAsync(p => p.Id == id, cancellation);
    }
}
