namespace OrderFlow.Application.Inventory.Exceptions;

public sealed class InventoryNotFoundException(Guid productId)
    : Exception($"Inventory for product '{productId}' was not found.");