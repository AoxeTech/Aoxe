using System;

namespace Zaaby.Abstractions
{
    public interface IZaabyMessageSubscriber
    {
        void Subscribe<TMessage>(Func<Action<TMessage>> handle);
    }
}