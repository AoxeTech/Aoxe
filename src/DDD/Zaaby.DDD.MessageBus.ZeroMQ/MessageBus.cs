using System;
using System.Threading.Tasks;
using Zaabee.ZeroMQ.Abstraction;
using Zaaby.DDD.Abstractions.Infrastructure.MessageBus;

namespace Zaaby.DDD.MessageBus.ZeroMQ
{
    public class MessageBus : IMessageBus, IDisposable
    {
        private readonly IZaabeeZeroMqHub _zeroMqHub;

        public MessageBus(IZaabeeZeroMqHub zeroMqHub)
        {
            _zeroMqHub = zeroMqHub;
        }

        public void Publish<T>(string topic, T message)
        {
            _zeroMqHub.Publish(topic, message);
        }

        public async Task PublishAsync<T>(string topic, T message)
        {
            await _zeroMqHub.PublishAsync(topic, message);
        }

        public void Dispose()
        {
            _zeroMqHub?.Dispose();
        }
    }
}