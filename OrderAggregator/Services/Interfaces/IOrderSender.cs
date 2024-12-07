namespace OrderAggregator.Services.Interfaces;

public interface IOrderSender
{
    Task SendOrdersAsync();
}