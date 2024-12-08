namespace OrderAggregator.Background.Options;

public class PeriodicOrderSenderOptions
{
    public int IntervalInSeconds { get; init; } = 30;
}