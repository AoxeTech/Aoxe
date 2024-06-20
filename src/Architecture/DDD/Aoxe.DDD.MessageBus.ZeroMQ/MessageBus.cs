using System;
using System.Threading.Tasks;
using Aoxe.ZeroMQ.Abstractions;
using Aoxe.DDD.Abstractions.Infrastructure.Messaging;

namespace Aoxe.DDD.MessageBus.ZeroMQ;

public class MessageBus : IMessageBus, IDisposable
{
    private readonly IAoxeZeroMessageBus _messageBus;

    public MessageBus(IAoxeZeroMessageBus zeroMessageBus) =>
        _messageBus = zeroMessageBus;

    public void Publish<T>(string topic, T message) =>
        _messageBus.Publish(topic, message);

    public async ValueTask PublishAsync<T>(string topic, T message) =>
        await _messageBus.PublishAsync(topic, message);

    public void Dispose() =>
        _messageBus.Dispose();
}