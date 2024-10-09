namespace Aoxe.Messaging.Abstractions;

public interface IDistributedEventBus
{
    ValueTask PublishAsync<TMessage>(TMessage message);
}
