using Zaaby.Core.Infrastructure.EventBus;

namespace Zaaby.Core.Domain
{
    public abstract class DomainEventHandler<TDomainEvent> : IDomainEventHandler where TDomainEvent : DomainEvent
    {
        public DomainEventHandler(IEventBus eventBus)
        {
            eventBus.SubscribeEvent<TDomainEvent>(Handle);
        }

        public abstract void Handle(TDomainEvent domainEvent);
    }
}