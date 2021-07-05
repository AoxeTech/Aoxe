namespace Zaaby.DDD.Abstractions.Domain
{
    public interface IAggregateRoot : IEntity
    {
    }

    public interface IAggregateRoot<out TId> : IEntity<TId>, IAggregateRoot
    {
    }

    public class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
    {
    }
}