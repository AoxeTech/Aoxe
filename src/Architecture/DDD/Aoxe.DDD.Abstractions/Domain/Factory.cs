namespace Aoxe.DDD.Abstractions.Domain;

public interface IFactory { }

public interface IFactory<TEntity> : IFactory
    where TEntity : IEntity { }

public class FactoryAttribute : Attribute { }
