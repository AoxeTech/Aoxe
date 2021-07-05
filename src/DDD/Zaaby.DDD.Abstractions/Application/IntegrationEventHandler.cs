using System;

namespace Zaaby.DDD.Abstractions.Application
{
    public interface IIntegrationEventHandler
    {
    }

    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IIntegrationEvent
    {
        void Handle(TIntegrationEvent integrationEvent);
    }

    public class IntegrationEventHandlerAttribute : Attribute
    {
        public string HandleName { get; }

        public IntegrationEventHandlerAttribute(string handleName)
        {
            if (string.IsNullOrWhiteSpace(handleName))
                throw new ArgumentNullException(nameof(handleName));
            HandleName = handleName;
        }
    }
}