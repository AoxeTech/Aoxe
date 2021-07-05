using System;
using Domain.AggregateRoots;
using Zaabee.SequentialGuid;
using Zaaby.DDD.Abstractions.Domain;

namespace Domain.DomainEvents
{
    public record UserNameChangedEvent : IDomainEvent<Guid>
    {
        public Guid Id { get; private set; }
        public Guid EntityId { get; private set; }
        public int EntityVersion { get; }
        public DateTime EventTime { get; private set; }
        public string Name { get; private set; }

        public UserNameChangedEvent(User user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            Id = SequentialGuidHelper.GenerateComb();
            EntityId = user.Id;
            EntityVersion = user.Version;
            Name = user.Name;
            EventTime = DateTime.UtcNow;
        }
    }
}