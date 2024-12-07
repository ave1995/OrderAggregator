using OrderAggregator.Models;
using OrderAggregator.Services;

namespace TestOrderAggregator;

[TestClass]
public class AddOrders
{
    [TestMethod]
    public async Task Test_AddOrders()
    {
        var orderManager = new OrderManager(new OrderStore());
        
        const int totalOrderCount = 100;
        
        for (var i = 0; i < totalOrderCount; i ++)
        {
            var orders = GenerateOrders(totalOrderCount);
            await orderManager.AddOrdersAsync(orders);
        }

        var totalOrders = await orderManager.GetAggregatedOrdersAsync();
        
        Assert.AreEqual(totalOrderCount, totalOrders.Count);
    }

    [TestMethod]
    public async Task Test_AddOrders_Parallel()
    {
        var orderManager = new OrderManager(new OrderStore());

        var tasks = new List<Task>();

        for (var i = 0; i < 1_000_000; i++)
        {
            tasks.Add(orderManager.AddOrdersAsync(GenerateOrders(i)));
        }

        await Task.WhenAll(tasks);

        var totalOrders = orderManager.GetAggregatedOrdersAsync();
    }
    
    private static IEnumerable<OrderItem> GenerateOrders(int count)
    {
        var rnd = new Random();
        for (var i = 1; i < 100 + 1; i++)
        {
            yield return new OrderItem(i, rnd.Next(1,1000));
        }
    }
}