using System;

namespace Zaaby.Core.Infrastructure.EventBus
{
    public interface IEventBus : IDisposable
    {
        void PublishEvent<T>(T @event) where T : IEvent;
        void PublishEvent(string eventName, byte[] body);
        void PublishMessage<T>(T message) where T : IMessage;

        /// <summary>
        /// The subscriber cluster will receive the event by the default queue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handle"></param>
        void ReceiveEvent<T>(Action<T> handle) where T : IEvent;

        /// <summary>
        /// The subscriber cluster will receive the event by its own queue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handle"></param>
        void SubscribeEvent<T>(Action<T> handle) where T : IEvent;

        /// <summary>
        /// The subscriber cluster will receive the message by the default queue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handle"></param>
        void ReceiveMessage<T>(Action<T> handle) where T : IMessage;

        /// <summary>
        /// The subscriber cluster will receive the message by its own queue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handle"></param>
        void SubscribeMessage<T>(Action<T> handle) where T : IMessage;

        /// <summary>
        /// The subscriber node will receive the message by its own queue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handle"></param>
        void ListenMessage<T>(Action<T> handle) where T : IMessage;
    }
}