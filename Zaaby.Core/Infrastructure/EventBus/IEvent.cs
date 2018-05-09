namespace Zaaby.Core.Infrastructure.EventBus
{
    /// <inheritdoc />
    /// <summary>
    /// This message type will be persisted and has dead letter queue.
    /// </summary>
    public interface IEvent : IMessage
    {
    }
}