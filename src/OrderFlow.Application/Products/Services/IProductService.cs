using OrderFlow.Application.Products.Dtos;
using OrderFlow.Application.Common;

namespace OrderFlow.Application.Products.Services
{
    public interface IProductService
    {
        Task<PagedResult<ProductResponse>> GetAllAsync(ProductQueryParameters parameters, CancellationToken cancellationToken = default);
        Task<ProductResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
        Task<ProductResponse> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default);
        Task<ProductResponse?> DeactivateAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ProductResponse?> ActivateAsync(Guid id, CancellationToken cancellationToken = default);
        //Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
