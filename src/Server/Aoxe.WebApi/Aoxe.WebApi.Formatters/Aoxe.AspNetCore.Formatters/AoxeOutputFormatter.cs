namespace Aoxe.AspNetCore.Formatters;

public class AoxeOutputFormatter : OutputFormatter
{
    private readonly IBytesSerializer _bytesSerializer;

    public AoxeOutputFormatter(MediaTypeHeaderValue contentType, IBytesSerializer bytesSerializer)
    {
        SupportedMediaTypes.Add(contentType);
        _bytesSerializer = bytesSerializer;
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context) =>
        await context
            .HttpContext
            .Response
            .BodyWriter
            .WriteAsync(_bytesSerializer.ToBytes(context.ObjectType!, context.Object));
}
