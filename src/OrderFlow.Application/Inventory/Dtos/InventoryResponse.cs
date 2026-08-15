namespace OrderFlow.Application.Inventory.Dtos;

public sealed record InventoryResponse(
    Guid ProductId,
    int Quantity);