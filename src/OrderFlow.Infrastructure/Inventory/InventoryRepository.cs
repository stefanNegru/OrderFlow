using Microsoft.EntityFrameworkCore;
using OrderFlow.Application.Inventory.Repositories;
using OrderFlow.Domain.Inventory;
using OrderFlow.Infrastructure.Persistence;

namespace OrderFlow.Infrastructure.Inventory;

public sealed class InventoryRepository(OrderFlowDbContext dbContext) : IInventoryRepository
{
    public Task<InventoryItem?> GetByProductIdAsync(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        return dbContext.InventoryItems
            .FirstOrDefaultAsync(i => i.ProductId == productId, cancellationToken);
    }

    public async Task AddAsync(
        InventoryItem inventoryItem,
        CancellationToken cancellationToken = default)
    {
        await dbContext.InventoryItems.AddAsync(inventoryItem, cancellationToken);
    }

    public async Task AddMovementAsync(
        StockMovement movement,
        CancellationToken cancellationToken = default)
    {
        await dbContext.StockMovements.AddAsync(
            movement,
            cancellationToken);
    }

    public async Task<IReadOnlyList<StockMovement>> GetMovementsAsync(
        Guid inventoryItemId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.StockMovements
            .AsNoTracking()
            .Where(i => i.InventoryItemId == inventoryItemId)
            .ToListAsync(cancellationToken);
    }

    public Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
