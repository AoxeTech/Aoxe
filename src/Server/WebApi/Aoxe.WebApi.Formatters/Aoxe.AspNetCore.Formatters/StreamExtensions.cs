namespace Aoxe.AspNetCore.Formatters;

public static class StreamExtensions
{
    public static async Task<byte[]> ReadToEndAsync(
        this Stream? stream,
        CancellationToken cancellationToken = default
    )
    {
        switch (stream)
        {
            case null:
                return Array.Empty<byte>();
            case MemoryStream ms:
                return ms.ToArray();
            default:
                await using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream, cancellationToken);
                    return memoryStream.ToArray();
                }
        }
    }
}
