using System;

namespace Zaaby.Abstractions
{
    public interface IZaabyMessageHub
    {
        void Publish<TMessage>(TMessage message);
        void Subscribe<TMessage>(Func<Action<TMessage>> handle);
    }
}