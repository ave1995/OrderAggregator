using OrderAggregator.Models;

namespace TestOrderAggregator;

public static class Helpers
{
    public static IEnumerable<OrderItem> GenerateOrders(int count)
    {
        for (var i = 1; i < count + 1; i++)
        {
            yield return new OrderItem(i, 10);
        }
    }
}