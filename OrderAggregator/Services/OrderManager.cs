using OrderAggregator.Models;
using OrderAggregator.Services.Interfaces;

namespace OrderAggregator.Services;

public class OrderManager(IOrderStore orderStore) : IOrderManager
{
    public async Task<IReadOnlyDictionary<int, int>> GetAggregatedOrdersAsync() => await orderStore.GetAggregatedOrdersAsync();
        
    public async Task AddOrdersAsync(IEnumerable<OrderItem> orders)
    {
        foreach (var order in orders)
            await orderStore.InsertOrderAsync(order);
    }
}