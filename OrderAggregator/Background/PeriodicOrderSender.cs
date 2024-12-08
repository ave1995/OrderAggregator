using Microsoft.Extensions.Options;
using OrderAggregator.Background.Options;
using OrderAggregator.Services.Interfaces;

namespace OrderAggregator.Background;

public class PeriodicOrderSender(IOptionsMonitor<PeriodicOrderSenderOptions> options, ILogger<PeriodicOrderSender> logger, IOrderSender orderSender) : BackgroundService
{
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(options.CurrentValue.IntervalInSeconds);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("{Service} is starting.", nameof(PeriodicOrderSender));
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var taskStartTime = DateTime.UtcNow;

            try
            {
                logger.LogInformation("{Task} started at: {Time}", nameof(orderSender.SendOrdersAsync), taskStartTime);
                await orderSender.SendOrdersAsync();
                logger.LogInformation("{Task} completed at: {Time}", nameof(orderSender.SendOrdersAsync), DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while executing {task}.", nameof(orderSender.SendOrdersAsync));
            }

            var durationOfTask = DateTime.UtcNow - taskStartTime;
            var delay = _interval - durationOfTask;
            
            if (delay <= TimeSpan.Zero) continue;
            
            logger.LogInformation("{Task} delaying for: {Delay}", nameof(orderSender.SendOrdersAsync), delay);
            await Task.Delay(delay, stoppingToken);
        }
    }
    
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("{Service} is stopping.", nameof(PeriodicOrderSender));
        return base.StopAsync(cancellationToken);
    }
}