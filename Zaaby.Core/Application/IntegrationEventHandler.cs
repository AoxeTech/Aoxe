using Zaaby.Core.Infrastructure.EventBus;

namespace Zaaby.Core.Application
{
    public abstract class IntegrationEventHandler<TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IIntegrationEvent
    {
        public IntegrationEventHandler(IEventBus eventBus)
        {
            eventBus.SubscribeEvent<TIntegrationEvent>(Handle);
        }

        public abstract void Handle(TIntegrationEvent domainEvent);
    }
}