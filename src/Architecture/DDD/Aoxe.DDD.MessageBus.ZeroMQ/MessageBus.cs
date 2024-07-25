namespace Aoxe.DDD.MessageBus.ZeroMQ;

public class MessageBus(IAoxeZeroMessageBus zeroMessageBus) : IMessageBus, IDisposable
{
    public void Publish<T>(string topic, T message) => zeroMessageBus.Publish(topic, message);

    public async ValueTask PublishAsync<T>(string topic, T message) =>
        await zeroMessageBus.PublishAsync(topic, message);

    public void Dispose() => zeroMessageBus.Dispose();
}
