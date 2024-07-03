namespace Aoxe.Core;

public record AoxeUnpublishedMessage : AoxeMessage
{
    public DateTime PersistenceUtcTime { get; protected set; } = DateTime.UtcNow;

    private AoxeUnpublishedMessage() { }

    public AoxeUnpublishedMessage(
        Guid id,
        string messageType,
        string content,
        DateTime persistenceUtcTime
    )
    {
        Id = id;
        MessageType = messageType;
        Content = content;
        PersistenceUtcTime = persistenceUtcTime;
    }
}
