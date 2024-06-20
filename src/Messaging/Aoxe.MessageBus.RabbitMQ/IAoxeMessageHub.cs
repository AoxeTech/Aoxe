namespace Aoxe.MessageBus.RabbitMQ;

public interface IAoxeMessageHub : IAoxeMessagePublisher, IAoxeMessageSubscriber
{
    void RegisterMessageSubscriber(
        Type messageHandlerInterfaceType,
        Type messageInterfaceType,
        string handleName
    );
}
