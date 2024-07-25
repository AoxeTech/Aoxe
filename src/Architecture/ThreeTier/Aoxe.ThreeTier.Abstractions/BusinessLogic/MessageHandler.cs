namespace Aoxe.ThreeTier.Abstractions.BusinessLogic;

public interface IMessageHandler { }

public interface IMessageHandler<in TMessage> : IMessageHandler
{
    void Handle(TMessage message);
}

public class MessageHandlerAttribute : Attribute
{
    public string HandleName { get; }

    public MessageHandlerAttribute(string handleName)
    {
        HandleName = handleName.Trim();
    }
}
