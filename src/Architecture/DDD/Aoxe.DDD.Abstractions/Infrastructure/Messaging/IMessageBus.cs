namespace Aoxe.DDD.Abstractions.Infrastructure.Messaging;

public interface IMessageBus
{
    void Publish<T>(string topic, T message);
    ValueTask PublishAsync<T>(string topic, T message);
}
