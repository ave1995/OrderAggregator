using System.Collections.Immutable;
using OrderAggregator.Models;

namespace OrderAggregator.Services.Interfaces;

public interface IOrderManager
{
    Task AddOrdersAsync(IEnumerable<OrderItem> orders);
    
    Task<IImmutableList<OrderItem>> GetOrdersAsync();
}