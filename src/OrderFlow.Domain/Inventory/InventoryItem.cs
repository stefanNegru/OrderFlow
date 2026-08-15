using OrderFlow.Domain.Inventory.Exceptions;

namespace OrderFlow.Domain.Inventory;

public sealed class InventoryItem
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    private InventoryItem() { }

    public InventoryItem(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Product id is required.", nameof(productId));
        }
        Id = Guid.NewGuid();
        ProductId = productId;
        Quantity = 0;
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        Quantity += quantity;
    }

    public void RemoveStock(int quantity) 
    {
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        if (quantity > Quantity) throw new InsufficientStockException(Quantity, quantity);
        Quantity -= quantity;
    }
}
