using OrderFlow.Application.Common;
using OrderFlow.Application.Products.Dtos;
using OrderFlow.Application.Products.Exceptions;
using OrderFlow.Application.Products.Repositories;
using OrderFlow.Domain.Products;

namespace OrderFlow.Application.Products.Services;

public sealed class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<PagedResult<ProductResponse>> GetAllAsync(ProductQueryParameters parameters, CancellationToken cancellation = default)
    {
        if (parameters.Page < 1)
        {
            throw new ArgumentException("Page number must be greater than zero");
        }

        if (parameters.PageSize < 1 || parameters.PageSize > 100)
        {
            throw new ArgumentException("PageSize must be between 1 and 100.");
        }

        var result = await productRepository.GetAllAsync(parameters, cancellation);
        var products = result.Items.Select(MapToResponse).ToList();
        return new PagedResult<ProductResponse>(
            products,
            result.Page,
            result.PageSize,
            result.TotalCount);
    }
    public async Task<ProductResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(id, cancellationToken);

        return product is null ? null : MapToResponse(product);
    }

    public async Task<ProductResponse> CreateAsync(
        CreateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        bool skuExists = await productRepository.ExistsBySkuAsync(request.Sku, cancellationToken);

        if (skuExists)
        {
            throw new DuplicateProductSkuException(request.Sku);
        }

        var product = new Product(
            request.Name,
            request.Sku,
            request.Price);

        await productRepository.AddAsync(product, cancellationToken);

        await productRepository.SaveChangesAsync(cancellationToken);

        return MapToResponse(product);
    }

    public async Task<ProductResponse> UpdateAsync(
        Guid id,
        UpdateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetTrackedByIdAsync(id, cancellationToken);

        if (product is null)
            return null;

        product.Update(request.Name, request.Price);
        await productRepository.SaveChangesAsync(cancellationToken);
        return MapToResponse(product);
    }

    public async Task<ProductResponse?> DeactivateAsync(Guid id, CancellationToken  cancellationToken = default)
    {
        var product = await productRepository.GetTrackedByIdAsync(id, cancellationToken);

        if (product is null)
            return null;

        product.Deactivate();

        await productRepository.SaveChangesAsync(cancellationToken);

        return MapToResponse(product);
    }

    public async Task<ProductResponse?> ActivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetTrackedByIdAsync(id, cancellationToken);

        if (product is null)
            return null;

        product.Activate();

        await productRepository.SaveChangesAsync(cancellationToken);

        return MapToResponse(product);
    }

    private static ProductResponse MapToResponse(Product product)
    {
        return new ProductResponse(
            product.Id,
            product.Name,
            product.Sku,
            product.Price,
            product.IsActive);
    }
}
