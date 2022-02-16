namespace Zaaby.DDD.Abstractions.Infrastructure.Messaging;

public interface IMessageBus
{
    void Publish<T>(string topic, T message);
    Task PublishAsync<T>(string topic, T message);
}