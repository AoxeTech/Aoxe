using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zaaby.Shared;
using Zaaby.DDD.Abstractions.Infrastructure.MessageBus;

namespace Zaaby.DDD
{
    public class DomainEventPublisher : BackgroundService
    {
        private readonly IMessageBus _messageBus;
        private readonly IServiceProvider _serviceProvider;

        public DomainEventPublisher(IMessageBus messageBus, IServiceProvider serviceProvider)
        {
            _messageBus = messageBus;
            _serviceProvider = serviceProvider.CreateScope().ServiceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await using var zaabyDddContext = _serviceProvider.GetService<ZaabyDddContext>();
            if (zaabyDddContext is null) throw new NullReferenceException(nameof(zaabyDddContext));
            var lastPublishTime = DateTime.UtcNow;
            while (!stoppingToken.IsCancellationRequested)
            {
                var ms = DateTime.UtcNow - lastPublishTime;
                if (ms.Milliseconds < 100)
                    await Task.Delay(100 - ms.Milliseconds, stoppingToken);

                var unpublishedMessages = zaabyDddContext.UnpublishedMessages
                    .OrderBy(p => p.PersistenceUtcTime)
                    .Take(100)
                    .ToList();

                foreach (var unpublishedMessage in unpublishedMessages)
                    await _messageBus.PublishAsync(unpublishedMessage.EventType, unpublishedMessage);

                zaabyDddContext.UnpublishedMessages.RemoveRange(unpublishedMessages);
                await zaabyDddContext.AddRangeAsync(
                    unpublishedMessages.Select(p => new PublishedMessage(p)), stoppingToken);

                await zaabyDddContext.SaveChangesAsync(stoppingToken);
                lastPublishTime = DateTime.UtcNow;
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_messageBus is IDisposable disposable) disposable.Dispose();
            await base.StopAsync(cancellationToken);
        }
    }
}