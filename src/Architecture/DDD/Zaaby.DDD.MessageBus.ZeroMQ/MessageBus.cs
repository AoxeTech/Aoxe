using System;
using System.Threading.Tasks;
using Zaabee.ZeroMQ.Abstractions;
using Zaaby.DDD.Abstractions.Infrastructure.Messaging;

namespace Zaaby.DDD.MessageBus.ZeroMQ;

public class MessageBus : IMessageBus, IDisposable
{
    private readonly IZaabeeZeroMessageBus _messageBus;

    public MessageBus(IZaabeeZeroMessageBus zeroMessageBus) =>
        _messageBus = zeroMessageBus;

    public void Publish<T>(string topic, T message) =>
        _messageBus.Publish(topic, message);

    public async ValueTask PublishAsync<T>(string topic, T message) =>
        await _messageBus.PublishAsync(topic, message);

    public void Dispose() =>
        _messageBus.Dispose();
}