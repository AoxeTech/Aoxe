namespace Aoxe.Shared;

public class PublishedMessage
{
    public Guid Id { get; set; }
    public string EventType { get; set; }
    public string Content { get; set; }
    public DateTime PersistenceUtcTime { get; set; }
    public DateTime PublishedUtcTime { get; set; }

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
