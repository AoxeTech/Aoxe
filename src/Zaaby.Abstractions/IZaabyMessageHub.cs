using System;

namespace Zaaby.Abstractions
{
    public interface IZaabyMessageHub : IZaabyMessagePublisher, IZaabyMessageSubscriber
    {
        void RegisterMessageSubscriber(Type messageHandlerInterfaceType, Type messageInterfaceType,
            string handleName);
    }
}