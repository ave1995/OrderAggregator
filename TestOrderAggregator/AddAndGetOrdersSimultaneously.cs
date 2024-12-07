using OrderAggregator.Models;
using OrderAggregator.Services;

namespace TestOrderAggregator;

[TestClass]
public class AddAndGetOrdersSimultaneously
{
    [TestMethod]
    public async Task Test_AddAndGetOrdersSimultaneously_ShouldAggregateOrdersCorrectly()
    {
        var orderManager = new OrderManager(new OrderStore());
        
        var orders = new List<OrderItem>
        {
            new(1, 5),
            new(2, 10),
            new(1, 3),
            new(3, 7)
        };

        var addOrdersTask = Task.WhenAll(
            Task.Run(() => orderManager.AddOrdersAsync(orders)),
            Task.Run(() => orderManager.AddOrdersAsync(orders)),
            Task.Run(() => orderManager.AddOrdersAsync(orders))
        );

        var getOrdersTask = Task.WhenAll(
            Task.Run(() => { orderManager.GetAggregatedOrdersAsync(); }),
            Task.Run(() => { orderManager.GetAggregatedOrdersAsync(); })
        );

        await Task.WhenAll(addOrdersTask, getOrdersTask);

        var finalAggregatedOrders = await orderManager.GetAggregatedOrdersAsync();
        Assert.AreEqual(24, finalAggregatedOrders[1]); 
        Assert.AreEqual(30, finalAggregatedOrders[2]); 
        Assert.AreEqual(21, finalAggregatedOrders[3]);
    }
    
    [TestMethod]
    public async Task Test_AddAndGetOrdersSimultaneously_ShouldAggregateOrdersCorrectly_Delays()
    {
        var orderManager = new OrderManager(new OrderStore());
    
        var orders = new List<OrderItem>
        {
            new(1, 5),
            new(2, 10),
            new(1, 3),
            new(3, 7)
        };

        var addOrdersTask = Task.WhenAll(
            Task.Run(async () => 
            {
                await Task.Delay(100);
                await orderManager.AddOrdersAsync(orders);
            }),
            Task.Run(async () => 
            {
                await Task.Delay(150);
                await orderManager.AddOrdersAsync(orders);
            }),
            Task.Run(async () => 
            {
                await Task.Delay(200);
                await orderManager.AddOrdersAsync(orders);
            })
        );

        var getOrdersTask = Task.WhenAll(
            Task.Run(async () => 
            {
                await Task.Delay(50);
                await orderManager.GetAggregatedOrdersAsync();
            }),
            Task.Run(async () => 
            {
                await Task.Delay(80);
                await orderManager.GetAggregatedOrdersAsync();
            })
        );

        await Task.WhenAll(addOrdersTask, getOrdersTask);

        var finalAggregatedOrders = await orderManager.GetAggregatedOrdersAsync();
        Assert.AreEqual(24, finalAggregatedOrders[1]); 
        Assert.AreEqual(30, finalAggregatedOrders[2]); 
        Assert.AreEqual(21, finalAggregatedOrders[3]);
    }
}