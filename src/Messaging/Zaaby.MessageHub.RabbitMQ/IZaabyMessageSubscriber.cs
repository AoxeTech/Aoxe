using System;

namespace Zaaby.MessageHub.RabbitMQ
{
    public interface IZaabyMessageSubscriber
    {
        void Subscribe<TMessage>(Func<Action<TMessage>> resolve);
    }
}