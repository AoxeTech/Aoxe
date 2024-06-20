namespace Aoxe.MessageBus.RabbitMQ;

public interface IAoxeMessagePublisher
{
    void Publish<TMessage>(TMessage message);
    Task PublishAsync<TMessage>(TMessage message);
}
