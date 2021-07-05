using System;
using Domain.AggregateRoots;
using Zaabee.SequentialGuid;
using Zaaby.DDD.Abstractions.Domain;

namespace Domain.DomainEvents
{
    public record UserCreatedEvent : IDomainEvent<Guid>
    {
        public Guid Id { get; private set; }
        public Guid EntityId { get; }
        public int EntityVersion { get; }
        public DateTime EventTime { get; private set; }
        public User User { get; private set; }

        public UserCreatedEvent(User user)
        {
            Id = SequentialGuidHelper.GenerateComb();
            User = user ?? throw new ArgumentNullException(nameof(user));
            EntityId = user.Id;
            EntityVersion = user.Version;
            EventTime = DateTime.UtcNow;
        }
    }
}