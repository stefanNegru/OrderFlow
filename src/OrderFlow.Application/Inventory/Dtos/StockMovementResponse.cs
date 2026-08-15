using OrderFlow.Domain.Inventory;

namespace OrderFlow.Application.Inventory.Dtos;

public sealed record StockMovementResponse(
    Guid Id,
    StockMovementType Type,
    int Quantity,
    DateTime CreatedAtUtc);