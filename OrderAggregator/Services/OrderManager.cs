using OrderAggregator.Models;
using OrderAggregator.Services.Interfaces;

namespace OrderAggregator.Services;

public class OrderManager : IOrderManager
{
    public Task AddOrdersAsync(IEnumerable<OrderItem> orders)
    {
        Console.WriteLine(string.Join(", ", orders));
        return Task.CompletedTask;
    }

    public IReadOnlyDictionary<int, int> GetAggregatedOrders()
    {
        throw new NotImplementedException();
    }
}