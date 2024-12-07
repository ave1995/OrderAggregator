using System.Collections.Concurrent;
using System.Collections.Immutable;
using OrderAggregator.Models;
using OrderAggregator.Services.Interfaces;

namespace OrderAggregator.Services;

public class OrderStore : IOrderStore
{
    private readonly ConcurrentDictionary<int, int> _aggregatedOrders;

    // ReSharper disable once ConvertConstructorToMemberInitializers
    public OrderStore()
    {
        _aggregatedOrders = new ConcurrentDictionary<int, int>();
    }

    /// <summary>
    /// Inserts an order item into the aggregated orders collection.
    /// If an item with the same ProductId already exists, it updates the quantity by adding the new quantity to the existing one.
    /// Otherwise, it adds the new order item with its quantity.
    /// </summary>
    /// <param name="order">The order item to insert, containing the product ID and quantity.</param>
    /// <returns>A completed task, indicating that the operation is synchronous and requires no asynchronous processing.</returns>
    public Task InsertOrderAsync(OrderItem order)
    {
        _aggregatedOrders.AddOrUpdate(
            order.ProductId,
            order.Quantity,
            (_, existingQuantity) => existingQuantity + order.Quantity);

        return Task.CompletedTask;
    }

    public Task<IImmutableList<OrderItem>> GetOrdersAsync()
    {
        var snapshot = _aggregatedOrders.Select(item
            => new OrderItem(item.Key, item.Value)).ToImmutableList();
        
        return Task.FromResult<IImmutableList<OrderItem>>(snapshot);
    }
}