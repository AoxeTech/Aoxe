namespace Aoxe.Core;

public class AoxeErrorMsg
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public DateTime ThrowTime { get; set; } = DateTime.UtcNow;
    public string Message { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string StackTrace { get; set; } = string.Empty;
}
