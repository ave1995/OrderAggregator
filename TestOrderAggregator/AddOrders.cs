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
        
        const int addOrdersCalls = 100_000;
        const int orderCount = 100;
        
        var orders = Helpers.GenerateOrders(orderCount).ToList();
        
        for (var i = 0; i < addOrdersCalls; i ++)
        {
            await orderManager.AddOrdersAsync(orders);
        }

        var totalOrders = await orderManager.GetOrdersAsync();
        
        Assert.AreEqual(100, totalOrders.Count);
        Assert.AreEqual(1_000_000, totalOrders[0].Quantity);
    }

    [TestMethod]
    public async Task Test_AddOrders_Parallel()
    {
        var orderManager = new OrderManager(new OrderStore());

        const int addOrdersCalls = 100_000;
        const int orderCount = 100;
        
        var orders = Helpers.GenerateOrders(orderCount).ToList();
        
        var tasks = new List<Task>();
        
        for (var i = 0; i < addOrdersCalls; i++)
        {
            tasks.Add(orderManager.AddOrdersAsync(orders));
        }

        await Task.WhenAll(tasks);

        var totalOrders = await orderManager.GetOrdersAsync();
        
        Assert.AreEqual(100, totalOrders.Count);
        Assert.AreEqual(1_000_000, totalOrders[0].Quantity);
    }
}