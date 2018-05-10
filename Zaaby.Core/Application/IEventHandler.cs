using Zaaby.Core.Infrastructure.EventBus;

namespace Zaaby.Core.Application
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        void Handle(TEvent @event);
    }
}