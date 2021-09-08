using System;

namespace Zaaby.ThreeTier.Annotations.BusinessLogic
{
    public interface IMessageHandler
    {
    }

    public interface IMessageHandler<in TMessage> : IMessageHandler
    {
        void Handle(TMessage message);
    }

    public class MessageHandlerAttribute : Attribute
    {
        public string HandleName { get; }

        public MessageHandlerAttribute(string handleName)
        {
            if (string.IsNullOrWhiteSpace(handleName))
                throw new ArgumentNullException(nameof(handleName));
            HandleName = handleName.Trim();
        }
    }
}