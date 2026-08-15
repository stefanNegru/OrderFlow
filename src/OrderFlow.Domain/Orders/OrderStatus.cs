namespace OrderFlow.Domain.Orders;

public enum OrderStatus
{
    Draft = 1,
    Confirmed = 2,
    Processing = 3,
    Shipped = 4,
    Delivered = 5,
    Cancelled = 6
}
