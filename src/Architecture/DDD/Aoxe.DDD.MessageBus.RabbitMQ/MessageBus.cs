namespace Aoxe.DDD.MessageBus.RabbitMQ;

public class MessageBus : IMessageBus, IDisposable
{
    private readonly IAoxeRabbitMqClient _rabbitMqClient;

    public MessageBus(IAoxeRabbitMqClient rabbitMqClient) => _rabbitMqClient = rabbitMqClient;

    public void Publish<T>(string topic, T message) => _rabbitMqClient.PublishEvent(topic, message);

    public ValueTask PublishAsync<T>(string topic, T message)
    {
        _rabbitMqClient.PublishEvent(topic, message);
#if NETSTANDARD2_0
        return new ValueTask();
#else
        return ValueTask.CompletedTask;
#endif
    }

    public void Dispose() => _rabbitMqClient.Dispose();
}
