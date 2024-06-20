using System;
using Domain.AggregateRoots;
using Aoxe.SequentialGuid;
using Aoxe.DDD.Abstractions.Domain;

namespace Domain.DomainEvents
{
    public record UserBirthdayCelebratedEvent : IDomainEvent<Guid>
    {
        public Guid Id { get; private set; }
        public Guid EntityId { get; private set; }
        public int EntityVersion { get; }
        public DateTime EventTime { get; private set; }
        public int Age { get; private set; }

        public UserBirthdayCelebratedEvent(User user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            Id = SequentialGuidHelper.GenerateComb();
            EntityId = user.Id;
            EntityVersion = user.Version;
            Age = user.Age;
            EventTime = DateTime.UtcNow;
        }
    }
}