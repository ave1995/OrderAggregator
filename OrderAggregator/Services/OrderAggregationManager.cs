using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using OrderAggregator.Models;
using OrderAggregator.Services.Interfaces;

namespace OrderAggregator.Services;

public class OrderAggregationManager : IOrderManager
{
    private readonly ConcurrentDictionary<int, int> _aggregatedOrders;

    // ReSharper disable once ConvertConstructorToMemberInitializers
    public OrderAggregationManager()
    {
        _aggregatedOrders = new ConcurrentDictionary<int, int>();
    }

    public IReadOnlyDictionary<int, int> GetAggregatedOrders() =>
        new ReadOnlyDictionary<int, int>(_aggregatedOrders);

    public async Task AddOrdersAsync(IEnumerable<OrderItem> orders)
    {
        var tasks = orders.Select(order => Task.Run(() =>
        {
            _aggregatedOrders.AddOrUpdate(
                order.ProductId,
                order.Quantity,
                (_, existingQuantity) => existingQuantity + order.Quantity);
        }));

        await Task.WhenAll(tasks);
    }
    
    public Task AddOrderAsync(OrderItem order)
    {
        _aggregatedOrders.AddOrUpdate(order.ProductId, order.Quantity,
            (_, existingQuantity) => existingQuantity + order.Quantity);
        
        return Task.CompletedTask;
    }

}