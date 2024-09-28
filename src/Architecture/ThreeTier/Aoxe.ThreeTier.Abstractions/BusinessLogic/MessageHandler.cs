namespace Aoxe.ThreeTier.Abstractions.BusinessLogic;

public interface IMessageHandler { }

public interface IMessageHandler<in TMessage> : IMessageHandler
{
    void Handle(TMessage message);
}

public class MessageHandlerAttribute(string handleName) : Attribute
{
    public string HandleName { get; } = handleName.Trim();
}
