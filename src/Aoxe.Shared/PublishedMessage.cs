namespace Aoxe.Shared;

public class PublishedMessage
{
    public Guid Id { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime PersistenceUtcTime { get; set; } = DateTime.UtcNow;
    public DateTime PublishedUtcTime { get; set; } = DateTime.UtcNow;

    private PublishedMessage() { }

    public PublishedMessage(UnpublishedMessage unpublishedMessage)
    {
        Id = unpublishedMessage.Id;
        EventType = unpublishedMessage.EventType;
        Content = unpublishedMessage.Content;
        PersistenceUtcTime = unpublishedMessage.PersistenceUtcTime;
        PublishedUtcTime = DateTime.UtcNow;
    }
}
