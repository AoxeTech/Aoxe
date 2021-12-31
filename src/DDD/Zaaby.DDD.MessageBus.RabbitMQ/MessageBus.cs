using System;
using System.Threading.Tasks;
using Zaabee.RabbitMQ.Abstractions;
using Zaaby.DDD.Abstractions.Infrastructure.MessageBus;

namespace Zaaby.DDD.MessageBus.RabbitMQ;

public class MessageBus : IMessageBus, IDisposable
{
    private readonly IZaabeeRabbitMqClient _rabbitMqClient;

    public MessageBus(IZaabeeRabbitMqClient rabbitMqClient) =>
        _rabbitMqClient = rabbitMqClient;

    public void Publish<T>(string topic, T message) =>
        _rabbitMqClient.PublishEvent(topic, message);

    public async Task PublishAsync<T>(string topic, T message) =>
        await _rabbitMqClient.PublishEventAsync(topic, message);

    public void Dispose() =>
        _rabbitMqClient.Dispose();
}