namespace OrderFlow.Application.Products.Dtos;

public sealed record UpdateProductRequest(
    string Name,
    decimal Price,
    bool IsActive);