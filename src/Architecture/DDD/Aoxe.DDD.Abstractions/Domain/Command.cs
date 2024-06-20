namespace Aoxe.DDD.Abstractions.Domain;

public interface ICommand : IValueObject { }

public abstract record Command : ICommand;
