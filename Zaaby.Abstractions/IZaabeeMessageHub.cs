using System;

namespace Zaaby.Abstractions
{
    public interface IZaabeeMessageHub
    {
        void Publish<TMessage>(TMessage @event);
        void Subscribe<TMessage>(Action<TMessage> handle);
        void Subscribe<TMessage>(Func<Action<TMessage>> handle);
        void Reset();
    }
}