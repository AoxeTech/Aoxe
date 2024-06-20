namespace Aoxe.Shared;

public class UnpublishedMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string EventType { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime PersistenceUtcTime { get; set; } = DateTime.UtcNow;
}
