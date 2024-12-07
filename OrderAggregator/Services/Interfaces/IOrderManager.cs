using OrderAggregator.Models;

namespace OrderAggregator.Services.Interfaces;

public interface IOrderManager
{
    Task AddOrdersAsync(IEnumerable<OrderItem> orders);
    IReadOnlyDictionary<int, int> GetAggregatedOrders();
}