using System;

namespace Zaaby.MessageHub.RabbitMQ
{
    public interface IZaabyMessageHub : IZaabyMessagePublisher, IZaabyMessageSubscriber
    {
        void RegisterMessageSubscriber(Type messageHandlerInterfaceType, Type messageInterfaceType,
            string handleName);
    }
}