namespace Zaaby.MessageBus.RabbitMQ;

public interface IZaabyMessageHub : IZaabyMessagePublisher, IZaabyMessageSubscriber
{
    void RegisterMessageSubscriber(Type messageHandlerInterfaceType, Type messageInterfaceType,
        string handleName);
}