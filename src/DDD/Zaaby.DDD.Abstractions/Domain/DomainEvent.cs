namespace Zaaby.DDD.Abstractions.Domain
{
    public interface IDomainEvent : IValueObject
    {
    }

    public interface IDomainEvent<out TEntityId> : IDomainEvent
    {
        TEntityId EntityId { get; }
        public int EntityVersion { get; }
    }

    public record DomainEvent<TEntityId> : IDomainEvent<TEntityId>
    {
        public TEntityId EntityId { get; protected set; }
        public int EntityVersion { get; protected set; }

        public DomainEvent(Entity<TEntityId> entity)
        {
            EntityId = entity.Id;
            EntityVersion = entity.Version;
        }
    }
}