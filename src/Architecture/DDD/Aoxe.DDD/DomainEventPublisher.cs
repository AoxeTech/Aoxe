namespace Aoxe.DDD;

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
        await using var AoxeDddContext = _serviceProvider.GetService<AoxeDddContext>();
        if (AoxeDddContext is null)
            throw new NullReferenceException(nameof(AoxeDddContext));
        var lastPublishTime = DateTime.UtcNow;
        while (!cancellationToken.IsCancellationRequested)
        {
            var ms = DateTime.UtcNow - lastPublishTime;
            if (ms.Milliseconds < 100)
                await Task.Delay(100 - ms.Milliseconds, cancellationToken);

            var unpublishedMessages = AoxeDddContext
                .UnpublishedMessages
                .OrderBy(p => p.PersistenceUtcTime)
                .Take(100)
                .ToList();

            foreach (var unpublishedMessage in unpublishedMessages)
                await _messageBus.PublishAsync(unpublishedMessage.EventType, unpublishedMessage);

            AoxeDddContext.UnpublishedMessages.RemoveRange(unpublishedMessages);
            await AoxeDddContext
                .PublishedMessages
                .AddRangeAsync(
                    unpublishedMessages.Select(p => new PublishedMessage(p)),
                    cancellationToken
                );

            await AoxeDddContext.SaveChangesAsync(cancellationToken);
            lastPublishTime = DateTime.UtcNow;
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_messageBus is IDisposable disposable)
            disposable.Dispose();
        if (_messageBus is IAsyncDisposable asyncDisposable)
            await asyncDisposable.DisposeAsync();
        await base.StopAsync(cancellationToken);
    }
}
