using Zaaby.Core.Infrastructure.EventBus;

namespace Zaaby.Core.Application
{
    public interface IMessageHandler<in TMessage> where TMessage : IMessage
    {
        void Handle(TMessage message);
    }
}