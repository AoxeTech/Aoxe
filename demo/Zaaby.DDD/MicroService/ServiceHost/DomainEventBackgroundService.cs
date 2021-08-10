using System.Threading;
using System.Threading.Tasks;
using Domain.DomainEvents;
using Microsoft.Extensions.Hosting;
using Zaabee.RabbitMQ.Abstractions;

namespace ServiceHost
{
    public class DomainEventBackgroundService : BackgroundService
    {
        private readonly IZaabeeRabbitMqClient _rabbitMqClient;

        public DomainEventBackgroundService(IZaabeeRabbitMqClient rabbitMqClient)
        {
            _rabbitMqClient = rabbitMqClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _rabbitMqClient.SubscribeEventAsync<UserBirthdayCelebratedEvent>(
                "Domain.DomainEvents.UserBirthdayCelebratedEvent", null);
            await _rabbitMqClient.SubscribeEventAsync<UserCardAddedEvent>(
                "Domain.DomainEvents.UserCardAddedEvent", null);
            await _rabbitMqClient.SubscribeEventAsync<UserCreatedEvent>(
                "Domain.DomainEvents.UserCreatedEvent", null);
            await _rabbitMqClient.SubscribeEventAsync<UserNameChangedEvent>(
                "Domain.DomainEvents.UserNameChangedEvent", null);
            await _rabbitMqClient.SubscribeEventAsync<UserTagsSetEvent>(
                "Domain.DomainEvents.UserTagsSetEvent", null);
        }
    }
}