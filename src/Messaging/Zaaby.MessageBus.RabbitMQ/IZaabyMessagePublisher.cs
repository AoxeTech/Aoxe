namespace Zaaby.MessageBus.RabbitMQ;

public interface IZaabyMessagePublisher
{
    void Publish<TMessage>(TMessage message);
    Task PublishAsync<TMessage>(TMessage message);
}