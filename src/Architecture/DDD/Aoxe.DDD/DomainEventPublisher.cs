namespace Aoxe.DDD;

public class DomainEventPublisher(IMessageBus messageBus, IServiceProvider serviceProvider)
    : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider
        .CreateScope()
        .ServiceProvider;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await using var aoxeDddContext = _serviceProvider.GetService<AoxeDddContext>();
        if (aoxeDddContext is null)
            throw new NullReferenceException(nameof(aoxeDddContext));
        var lastPublishTime = DateTime.UtcNow;
        while (!cancellationToken.IsCancellationRequested)
        {
            var ms = DateTime.UtcNow - lastPublishTime;
            if (ms.Milliseconds < 100)
                await Task.Delay(100 - ms.Milliseconds, cancellationToken);

            var unpublishedMessages = aoxeDddContext
                .UnpublishedMessages.OrderBy(p => p.PersistenceUtcTime)
                .Take(100)
                .ToList();

            foreach (var unpublishedMessage in unpublishedMessages)
                await messageBus.PublishAsync(unpublishedMessage.MessageType, unpublishedMessage);

            aoxeDddContext.UnpublishedMessages.RemoveRange(unpublishedMessages);
            await aoxeDddContext.PublishedMessages.AddRangeAsync(
                unpublishedMessages.Select(p => new AoxePublishedMessage(p)),
                cancellationToken
            );

            await aoxeDddContext.SaveChangesAsync(cancellationToken);
            lastPublishTime = DateTime.UtcNow;
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (messageBus is IDisposable disposable)
            disposable.Dispose();
        if (messageBus is IAsyncDisposable asyncDisposable)
            await asyncDisposable.DisposeAsync();
        await base.StopAsync(cancellationToken);
    }
}
