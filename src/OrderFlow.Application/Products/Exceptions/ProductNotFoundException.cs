namespace OrderFlow.Application.Products.Exceptions;

public sealed class ProductNotFoundException(Guid productId)
    : Exception($"Product with ID '{productId}' was not found.");