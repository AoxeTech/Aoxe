using System;
using Domain.AggregateRoots;
using Domain.Entities;
using Zaabee.SequentialGuid;
using Zaaby.DDD.Abstractions.Domain;

namespace Domain.DomainEvents
{
    public record UserCardAddedEvent : IDomainEvent<Guid>
    {
        public Guid Id { get; private set; }
        public Guid EntityId { get; private set; }
        public int EntityVersion { get; }
        public DateTime EventTime { get; private set; }
        public Guid CardId { get; private set; }
        public string CardName { get; private set; }

        public UserCardAddedEvent(User user, Card card)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            if (card is null) throw new ArgumentNullException(nameof(card));
            Id = SequentialGuidHelper.GenerateComb();
            EntityId = user.Id;
            EntityVersion = user.Version;
            CardId = card.Id;
            CardName = card.Name;
            EventTime = DateTime.UtcNow;
        }
    }
}