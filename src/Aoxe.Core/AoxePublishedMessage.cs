namespace Aoxe.Core;

public record AoxePublishedMessage : AoxeMessage
{
    public DateTime PersistenceUtcTime { get; protected set; } = DateTime.UtcNow;
    public DateTime PublishedUtcTime { get; protected set; } = DateTime.UtcNow;

    private AoxePublishedMessage() { }

    public AoxePublishedMessage(AoxeUnpublishedMessage unpublishedMessage)
    {
        Id = unpublishedMessage.Id;
        MessageType = unpublishedMessage.MessageType;
        Content = unpublishedMessage.Content;
        PersistenceUtcTime = unpublishedMessage.PersistenceUtcTime;
        PublishedUtcTime = DateTime.UtcNow;
    }
}
