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
            _rabbitMqClient.SubscribeEvent<UserBirthdayCelebratedEvent>(
                "Domain.DomainEvents.UserBirthdayCelebratedEvent", null);
            _rabbitMqClient.SubscribeEvent<UserCardAddedEvent>(
                "Domain.DomainEvents.UserCardAddedEvent", null);
            _rabbitMqClient.SubscribeEvent<UserCreatedEvent>(
                "Domain.DomainEvents.UserCreatedEvent", null);
            _rabbitMqClient.SubscribeEvent<UserNameChangedEvent>(
                "Domain.DomainEvents.UserNameChangedEvent", null);
            _rabbitMqClient.SubscribeEvent<UserTagsSetEvent>(
                "Domain.DomainEvents.UserTagsSetEvent", null);
        }
    }
}