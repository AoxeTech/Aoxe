using Zaaby.Core.Infrastructure.EventBus;

namespace Zaaby.Core.Domain
{
    public interface IDomainEvent<TId> : IEvent, IEntity<TId>
    {

    }
}