namespace Zaaby.Core
{
    public interface IZaabyRepository<TAggregateRoot, TId> where TAggregateRoot : IAggregateRoot<TId>
    {
    }
}