using System;

namespace Zaaby.Core.Domain
{
    public class DomainEvent : IDomainEvent
    {
        public Guid Id { get; protected set; }
        public DateTimeOffset Timestamp { get; protected set; }
    }
}