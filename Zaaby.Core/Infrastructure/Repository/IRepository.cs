using Zaaby.Core.Domain;

namespace Zaaby.Core.Infrastructure.Repository
{
    public interface IRepository<TAggregateRoot, TId> where TAggregateRoot : IAggregateRoot<TId>
    {
    }
}