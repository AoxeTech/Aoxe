namespace Aoxe.DDD.Abstractions.Domain;

public interface IAggregateRoot : IEntity { }

public interface IAggregateRoot<out TId> : IEntity<TId>, IAggregateRoot { }

public abstract class AggregateRoot : Entity, IAggregateRoot { }

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot { }
