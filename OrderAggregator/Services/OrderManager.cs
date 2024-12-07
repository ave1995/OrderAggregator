using System.Collections.Immutable;
using OrderAggregator.Models;
using OrderAggregator.Services.Interfaces;

namespace OrderAggregator.Services;

public class OrderManager(IOrderStore orderStore) : IOrderManager
{
    public async Task<IImmutableList<OrderItem>> GetOrdersAsync()
    {
        var aggregatedOrders = await orderStore.GetAggregatedOrdersAsync();
        return aggregatedOrders.Select(item
            => new OrderItem(item.Key, item.Value)).ToImmutableList();
    }

    public async Task AddOrdersAsync(IEnumerable<OrderItem> orders)
    {
        foreach (var order in orders)
            await orderStore.InsertOrderAsync(order);
    }
}