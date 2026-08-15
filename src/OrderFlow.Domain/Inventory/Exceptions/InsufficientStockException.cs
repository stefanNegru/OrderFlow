namespace OrderFlow.Domain.Inventory.Exceptions;

public sealed class InsufficientStockException(int available, int requested)
    : Exception($"Insufficient stock. Available: {available}, requested: {requested}.");