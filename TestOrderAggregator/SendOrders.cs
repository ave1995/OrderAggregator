using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using OrderAggregator.Services;
using OrderAggregator.Services.Interfaces;

namespace TestOrderAggregator;

[TestClass]
public class SendOrders
{
    private OrderManager _orderManager = default!;
    private ILogger<OrderSender> _logger = default!;
    private IServiceProvider _serviceProvider = default!;
    private const int OrderCount = 1;
    private const string OrdersJsonAssertResult = "[\n  {\n    \"productId\": 1,\n    \"quantity\": 10\n  }\n]";
    
    [TestInitialize]
    public void Initialize()
    {
        _orderManager = new OrderManager(new OrderStore());
        
        var mockServiceProvider = new Mock<IServiceProvider>();
        var mockScopeFactory = new Mock<IServiceScopeFactory>();
        var mockScope = new Mock<IServiceScope>();

        mockServiceProvider.Setup(sp => sp.GetService(typeof(IServiceScopeFactory)))
            .Returns(mockScopeFactory.Object);

        mockScopeFactory.Setup(factory => factory.CreateScope())
            .Returns(mockScope.Object);

        mockScope.Setup(scope => scope.ServiceProvider.GetService(typeof(IOrderManager)))
            .Returns(_orderManager);
        
        _serviceProvider = mockServiceProvider.Object;
        _logger = new Mock<ILogger<OrderSender>>().Object;

        var orders = Helpers.GenerateOrders(OrderCount).ToList();

        _orderManager.AddOrdersAsync(orders).Wait();
    }
    
    
    [TestMethod]
    public async Task Test_SendOrdersAsync()
    {
        var sender = new OrderSender(_logger, _serviceProvider);
        var result = await sender.SendOrdersAsync();
        
        Assert.AreEqual(OrdersJsonAssertResult, result);
    }
}