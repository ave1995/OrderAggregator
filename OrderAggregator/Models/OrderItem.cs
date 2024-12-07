namespace OrderAggregator.Models;

public readonly struct OrderItem(int productId, int quantity)
{
    public int ProductId { get; } = productId;
    public int Quantity { get; } = quantity;
}