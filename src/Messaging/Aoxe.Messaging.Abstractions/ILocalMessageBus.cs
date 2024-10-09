namespace Aoxe.Messaging.Abstractions;

public interface ILocalMessageBus
{
    void Publish<TMessage>(TMessage message);
}
