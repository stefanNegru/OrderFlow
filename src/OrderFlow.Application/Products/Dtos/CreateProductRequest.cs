namespace OrderFlow.Application.Products.Dtos;

public sealed record CreateProductRequest(
    string Name,
    string Sku,
    decimal Price);