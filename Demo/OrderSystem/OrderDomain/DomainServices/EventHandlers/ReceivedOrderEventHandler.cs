using OrderDomain.DomainEvents;
using Zaaby.Core.Domain;
using Zaaby.Core.Infrastructure.EventBus;

namespace OrderDomain.DomainServices.EventHandlers
{
    public class ReceivedOrderEventHandler : DomainEventHandler<ReceivedOrderEvent>
    {
        public ReceivedOrderEventHandler(IEventBus eventBus) : base(eventBus)
        {
        }

        public override void Handle(ReceivedOrderEvent domainEvent)
        {

        }
    }
}