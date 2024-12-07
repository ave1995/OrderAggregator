using OrderAggregator.Models;

namespace OrderAggregator.Services.Interfaces;

public interface IOrderManager
{
    Task AddOrdersAsync(IEnumerable<OrderItem> orders);
    Task AddOrderAsync(OrderItem order);
    IReadOnlyDictionary<int, int> GetAggregatedOrders();
}