namespace Zaaby.DDD.Abstractions.Domain
{
    public interface ICommand : IValueObject
    {
    }

    public record Command : ICommand;
}