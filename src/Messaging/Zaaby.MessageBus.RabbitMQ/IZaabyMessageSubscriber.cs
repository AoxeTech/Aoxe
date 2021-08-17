using System;

namespace Zaaby.MessageBus.RabbitMQ
{
    public interface IZaabyMessageSubscriber
    {
        void Subscribe<TMessage>(Func<Action<TMessage>> resolve);
    }
}