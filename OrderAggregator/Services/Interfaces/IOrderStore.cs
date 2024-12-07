using System.Collections.Immutable;
using OrderAggregator.Models;

namespace OrderAggregator.Services.Interfaces;

public interface IOrderStore
{
    Task InsertOrderAsync(OrderItem order);
    
    Task<IImmutableList<OrderItem>> GetOrdersAsync();
}