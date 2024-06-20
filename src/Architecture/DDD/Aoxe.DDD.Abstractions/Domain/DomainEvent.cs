namespace Aoxe.DDD.Abstractions.Domain;

public interface IDomainEvent : IValueObject { }

public interface IDomainEvent<out TEntityId> : IDomainEvent
{
    TEntityId EntityId { get; }
    public int EntityVersion { get; }
}

public abstract record DomainEvent<TEntityId> : IDomainEvent<TEntityId>
{
    public TEntityId EntityId { get; protected set; }
    public int EntityVersion { get; protected set; }

    protected DomainEvent(Entity<TEntityId> entity)
    {
        EntityId = entity.Id;
        EntityVersion = entity.Version;
    }
}
