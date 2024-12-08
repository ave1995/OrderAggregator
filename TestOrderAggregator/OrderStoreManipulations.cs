using OrderAggregator.Models;
using OrderAggregator.Services;

namespace TestOrderAggregator;

[TestClass]
public class OrderStoreManipulations
{
    /// <summary>
    /// AddAndUpdate Task with Clear Task in parallel order to check data consistency.
    /// </summary>
    [TestMethod]
    public async Task Test_AddUpdateAndClear_Parallel()
    {
        var store = new OrderStore();

        var tasks = new List<Task>();

        const int overallQuantity = 100;

        for (var i = 0; i < overallQuantity; i++)
        {
            tasks.Add(Task.Run(async () =>
            {
                await store.InsertOrderAsync(new OrderItem(1, 1));
                await Task.Delay(1);
            }));
        }

        var taskSnapshot = store.ClearOrdersAndGetSnapshotAsync();

        tasks.Add(Task.Run(async () =>
        {
            await Task.Delay(10);
            return await taskSnapshot;
        }));

        await Task.WhenAll(tasks);

        var snapshotResult = await taskSnapshot;

        var ordersAfterClear = await store.GetCurrentOrdersAsync();

        var snapshotOrderQuantity = snapshotResult.Any() ? snapshotResult[0].Quantity : 0;
        var orderQuantityAfterClear = ordersAfterClear.Any() ? ordersAfterClear[0].Quantity : 0;
        
        Console.WriteLine(snapshotOrderQuantity);
        Console.WriteLine(orderQuantityAfterClear);
        
        Assert.AreEqual(overallQuantity, snapshotOrderQuantity + orderQuantityAfterClear);
    }
}