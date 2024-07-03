namespace Aoxe.Core;

public abstract record AoxeMessage
{
    public Guid Id { get; protected set; }
    public string MessageType { get; protected set; } = string.Empty;
    public string Content { get; protected set; } = string.Empty;
}
