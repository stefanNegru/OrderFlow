using OrderFlow.Domain.Inventory;

namespace OrderFlow.Application.Inventory.Repositories;

public interface IInventoryRepository
{
    Task<InventoryItem?> GetByProductIdAsync(
        Guid productId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        InventoryItem inventoryItem,
        CancellationToken cancellationToken = default);

    Task AddMovementAsync(
        StockMovement movement,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<StockMovement>> GetMovementsAsync(
        Guid inventoryItemId,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}