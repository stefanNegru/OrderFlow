using System;
using System.Collections.Generic;
using System.Text;

namespace OrderFlow.Domain.Inventory;

public sealed class StockMovement
{
    public Guid Id { get; private set; }
    public Guid InventoryItemId { get; private set; }
    public StockMovementType Type { get; private set; }
    public int Quantity { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    private StockMovement()
    {
    }

    public StockMovement(
        Guid inventoryItemId,
        StockMovementType type,
        int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity));
        }

        Id = Guid.NewGuid();
        InventoryItemId = inventoryItemId;
        Type = type;
        Quantity = quantity;
        CreatedAtUtc = DateTime.UtcNow;
    }
}
