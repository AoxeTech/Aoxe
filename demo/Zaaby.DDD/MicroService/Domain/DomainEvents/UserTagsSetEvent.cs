using System;
using System.Collections.Generic;
using System.Linq;
using Domain.AggregateRoots;
using Zaabee.SequentialGuid;
using Zaaby.DDD.Abstractions.Domain;

namespace Domain.DomainEvents
{
    public record UserTagsSetEvent : IDomainEvent<Guid>
    {
        public Guid Id { get; private set; }
        public Guid EntityId { get; private set; }
        public int EntityVersion { get; }
        public DateTime EventTime { get; private set; }
        public List<string> Tags { get; private set; }

        public UserTagsSetEvent(User user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            Id = SequentialGuidHelper.GenerateComb();
            EntityId = user.Id;
            EntityVersion = user.Version;
            Tags = user.Tags.ToList();
            EventTime = DateTime.UtcNow;
        }
    }
}