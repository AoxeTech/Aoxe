namespace Aoxe.DDD.Abstractions.Infrastructure.Repository;

public interface IRepository { }

public interface IRepository<in TAggregateRoot> : IRepository
    where TAggregateRoot : IAggregateRoot { }

public interface IRepository<in TAggregateRoot, in TId> : IRepository<TAggregateRoot>
    where TAggregateRoot : IAggregateRoot<TId> { }

public class RepositoryAttribute : Attribute { }
