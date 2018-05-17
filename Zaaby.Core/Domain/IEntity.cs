namespace Zaaby.Core.Domain
{
    public interface IEntity<out TId> : IEntity
    {
        TId Id { get; }
    }

    public interface IEntity
    {
    }
}