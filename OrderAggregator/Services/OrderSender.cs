using System.Text.Json;
using OrderAggregator.Services.Interfaces;

namespace OrderAggregator.Services;

public class OrderSender(ILogger<OrderSender> logger, IServiceProvider serviceProvider) : IOrderSender
{
    private static readonly JsonSerializerOptions JsonWriteOptions = new()
    {
        WriteIndented = true
    };

    public async Task<string> SendOrdersAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var orderManager = scope.ServiceProvider.GetRequiredService<IOrderManager>();
        var orders = await orderManager.GetOrdersAsync();

        var json = JsonSerializer.Serialize(orders, JsonWriteOptions);

        logger.LogInformation("{json}", json);
        
        return json;
    }
}