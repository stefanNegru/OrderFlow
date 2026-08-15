using OrderFlow.Application.Inventory.Dtos;
using OrderFlow.Application.Inventory.Exceptions;
using OrderFlow.Application.Inventory.Repositories;
using OrderFlow.Application.Products.Exceptions;
using OrderFlow.Application.Products.Repositories;
using OrderFlow.Domain.Inventory;

namespace OrderFlow.Application.Inventory.Services;

public sealed class InventoryService(
    IInventoryRepository inventoryRepository,
    IProductRepository productRepository) : IInventoryService
{
    public async Task<InventoryResponse?> GetAsync(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        var inventory = await inventoryRepository.GetByProductIdAsync(productId, cancellationToken);

        return inventory is null
            ? null
            : Map(inventory);
    }

    public async Task<InventoryResponse> AddStockAsync(
        Guid productId,
        AddStockRequest request,
        CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(
            productId,
            cancellationToken);

        if (product is null)
            throw new ProductNotFoundException(productId);

        var inventory = await inventoryRepository.GetByProductIdAsync(
            productId,
            cancellationToken);

        if (inventory is null)
        {
            inventory = new InventoryItem(productId);
            await inventoryRepository.AddAsync(inventory, cancellationToken);
        }

        inventory.AddStock(request.Quantity);

        var movement = new StockMovement(
            inventory.Id,
            StockMovementType.Added,
            request.Quantity);

        await inventoryRepository.AddMovementAsync(movement, cancellationToken);
        await inventoryRepository.SaveChangesAsync(cancellationToken);

        return Map(inventory);
    }

    public async Task<InventoryResponse> RemoveStockAsync(
        Guid productId,
        RemoveStockRequest request,
        CancellationToken cancellationToken = default)
    {
        var inventory = await inventoryRepository.GetByProductIdAsync(productId, cancellationToken);

        if (inventory is null)
            throw new InventoryNotFoundException(productId);

        inventory.RemoveStock(request.Quantity);

        var movement = new StockMovement(
            inventory.Id,
            StockMovementType.Removed,
            request.Quantity);

        await inventoryRepository.AddMovementAsync(movement, cancellationToken);

        await inventoryRepository.SaveChangesAsync(cancellationToken);

        return Map(inventory);
    }

    public async Task<IReadOnlyList<StockMovementResponse>> GetMovementsAsync(
            Guid productId,
            CancellationToken cancellationToken = default)
    {
        var inventory = await inventoryRepository.GetByProductIdAsync(productId, cancellationToken);

        if (inventory is null)
            return [];

        var movements = await inventoryRepository.GetMovementsAsync(inventory.Id, cancellationToken);

        return movements
            .Select(x => new StockMovementResponse(
                x.Id,
                x.Type,
                x.Quantity,
                x.CreatedAtUtc))
            .ToList();
    }

    private static InventoryResponse Map(
        InventoryItem inventory)
    {
        return new InventoryResponse(
            inventory.ProductId,
            inventory.Quantity);
    }
}
