using System.Collections.Concurrent;
using System.Collections.Immutable;
using OrderAggregator.Models;
using OrderAggregator.Services.Interfaces;

namespace OrderAggregator.Services;

public class OrderStore : IOrderStore
{
    private readonly Dictionary<int, int> _aggregatedOrders;

    private readonly ReaderWriterLockSlim _lock = new();

    // ReSharper disable once ConvertConstructorToMemberInitializers
    public OrderStore()
    {
        _aggregatedOrders = new Dictionary<int, int>();
    }

    /// <summary>
    /// Inserts an order item into the aggregated orders collection.
    /// If an item with the same ProductId already exists, it updates the quantity by adding the new quantity to the existing one.
    /// Otherwise, it adds the new order item with its quantity.
    /// </summary>
    /// <param name="order">The order item to insert, containing the product ID and quantity.</param>
    /// <returns>A completed task, indicating that the operation is synchronous and requires no asynchronous processing.</returns>
    public Task InsertOrderAsync(OrderItem order)
    {
        _lock.EnterWriteLock();

        try
        {
            if (!_aggregatedOrders.TryAdd(order.ProductId, order.Quantity))
                _aggregatedOrders[order.ProductId] += order.Quantity;
        }
        finally
        {
            _lock.ExitWriteLock();
        }

        return Task.CompletedTask;
    }

    public Task<IImmutableList<OrderItem>> GetCurrentOrdersAsync()
        => Task.FromResult<IImmutableList<OrderItem>>(GetImmutableOrders(_aggregatedOrders));

    public Task<IImmutableList<OrderItem>> ClearOrdersAndGetSnapshotAsync()
    {
        _lock.EnterWriteLock();
        try
        {
            var oldData = new Dictionary<int, int>(_aggregatedOrders);
            _aggregatedOrders.Clear();

            return Task.FromResult<IImmutableList<OrderItem>>(GetImmutableOrders(oldData));
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    private static ImmutableList<OrderItem> GetImmutableOrders(IDictionary<int, int> orders) => orders
        .Select(item => new OrderItem(item.Key, item.Value))
        .ToImmutableList();
}