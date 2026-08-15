using OrderFlow.Application.Common;
using OrderFlow.Application.Products.Dtos;
using OrderFlow.Domain.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderFlow.Application.Products.Repositories
{
    public interface IProductRepository
    {
        Task<PagedResult<Product>> GetAllAsync(ProductQueryParameters parameters, CancellationToken cancellation = default);
        Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellation = default);
        Task<bool> ExistsBySkuAsync(string sku, CancellationToken cancellation = default);
        Task AddAsync(Product product, CancellationToken cancellation = default);
        Task SaveChangesAsync(CancellationToken cancellation = default);
        Task<Product?> GetTrackedByIdAsync(Guid id, CancellationToken cancellation = default);
    }
}
