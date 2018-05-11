using System;

namespace Zaaby.Core.Infrastructure.EventBus
{
    /// <summary>
    /// The base interface of message.
    /// </summary>
    public interface IMessage
    {
        DateTimeOffset Timestamp { get; }
    }
}