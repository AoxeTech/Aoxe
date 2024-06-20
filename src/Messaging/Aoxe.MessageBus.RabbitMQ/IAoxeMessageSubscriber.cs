namespace Aoxe.MessageBus.RabbitMQ;

public interface IAoxeMessageSubscriber
{
    void Subscribe<TMessage>(Func<Action<TMessage>> resolve);
}
