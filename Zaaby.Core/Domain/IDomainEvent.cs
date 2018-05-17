using System;
using Zaaby.Core.Infrastructure.EventBus;

namespace Zaaby.Core.Domain
{
    public interface IDomainEvent : IEvent, IEntity<Guid>
    {
    }
}