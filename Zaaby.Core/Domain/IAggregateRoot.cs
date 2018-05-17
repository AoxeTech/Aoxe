namespace Zaaby.Core.Domain
{
    public interface IAggregateRoot<out TId> : IEntity<TId>
    {

    }
}