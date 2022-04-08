namespace Zaaby.AspNetCore.Formatters;

public class ZaabyOutputFormatter : OutputFormatter
{
    private readonly IBytesSerializer _bytesSerializer;

    public ZaabyOutputFormatter(MediaTypeHeaderValue contentType, IBytesSerializer bytesSerializer)
    {
        SupportedMediaTypes.Add(contentType);
        _bytesSerializer = bytesSerializer;
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context) =>
        await context.HttpContext.Response.BodyWriter.WriteAsync(
            _bytesSerializer.ToBytes(
                context.ObjectType!,
                context.Object));
}