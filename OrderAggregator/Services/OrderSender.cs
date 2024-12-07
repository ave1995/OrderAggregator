using System.Text.Json;
using OrderAggregator.Models;
using OrderAggregator.Services.Interfaces;

namespace OrderAggregator.Services;

public class OrderSender(ILogger<OrderSender> logger, IServiceProvider serviceProvider) : IOrderSender
{
    private static readonly JsonSerializerOptions JsonWriteOptions = new()
    {
        WriteIndented = true
    };

    public async Task SendOrdersAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var orderManager = scope.ServiceProvider.GetRequiredService<IOrderManager>();
        var orders = await orderManager.GetAggregatedOrdersAsync();

        var jsonObject = orders.Select(item
                => new OrderItem(item.Key, item.Value))
            .ToList();

        var json = JsonSerializer.Serialize(jsonObject, JsonWriteOptions);

        logger.LogInformation("{json}", json);
    }
}