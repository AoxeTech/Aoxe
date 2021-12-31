namespace Zaaby.DDD;

public class DomainEventPublisher : BackgroundService
{
    private readonly IMessageBus _messageBus;
    private readonly IServiceProvider _serviceProvider;

    public DomainEventPublisher(IMessageBus messageBus, IServiceProvider serviceProvider)
    {
        _messageBus = messageBus;
        _serviceProvider = serviceProvider.CreateScope().ServiceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await using var zaabyDddContext = _serviceProvider.GetService<ZaabyDddContext>();
        if (zaabyDddContext is null) throw new NullReferenceException(nameof(zaabyDddContext));
        var lastPublishTime = DateTime.UtcNow;
        while (!cancellationToken.IsCancellationRequested)
        {
            var ms = DateTime.UtcNow - lastPublishTime;
            if (ms.Milliseconds < 100)
                await Task.Delay(100 - ms.Milliseconds, cancellationToken);

            var unpublishedMessages = zaabyDddContext.UnpublishedMessages
                .OrderBy(p => p.PersistenceUtcTime)
                .Take(100)
                .ToList();

            foreach (var unpublishedMessage in unpublishedMessages)
                await _messageBus.PublishAsync(unpublishedMessage.EventType, unpublishedMessage);

            zaabyDddContext.UnpublishedMessages.RemoveRange(unpublishedMessages);
            await zaabyDddContext.PublishedMessages.AddRangeAsync(
                unpublishedMessages.Select(p => new PublishedMessage(p)), cancellationToken);

            await zaabyDddContext.SaveChangesAsync(cancellationToken);
            lastPublishTime = DateTime.UtcNow;
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_messageBus is IDisposable disposable) disposable.Dispose();
        if (_messageBus is IAsyncDisposable asyncDisposable) await asyncDisposable.DisposeAsync();
        await base.StopAsync(cancellationToken);
    }
}