namespace Zaaby.Core.Infrastructure.EventBus
{
    /// <inheritdoc />
    /// <summary>
    /// This message type will republish to dead letter queue when throw a exception.
    /// </summary>
    public interface IEvent : IMessage
    {
    }
}