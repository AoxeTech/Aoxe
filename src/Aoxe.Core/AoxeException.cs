namespace Aoxe.Core;

public class AoxeException : Exception
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; } = string.Empty;
    public DateTime ThrowTime { get; set; } = DateTime.UtcNow;

    public AoxeException() { }

    public AoxeException(string message)
        : base(message) { }

    public AoxeException(string message, Exception innerException)
        : base(message, innerException) { }
}
