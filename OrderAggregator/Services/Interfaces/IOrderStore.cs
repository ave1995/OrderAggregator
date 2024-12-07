using OrderAggregator.Models;

namespace OrderAggregator.Services.Interfaces;

public interface IOrderStore
{
    Task InsertOrderAsync(OrderItem order);
    
    Task<IReadOnlyDictionary<int, int>> GetAggregatedOrdersAsync();
}