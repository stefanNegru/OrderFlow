namespace OrderFlow.Application.Products.Exceptions;

public sealed class DuplicateProductSkuException(string sku)
    : Exception($"A product with SKU '{sku}' already exists.");