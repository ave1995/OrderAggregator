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
    /// Objednávka se buď vytváří nebo se jen aktualizuje quantity.
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
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