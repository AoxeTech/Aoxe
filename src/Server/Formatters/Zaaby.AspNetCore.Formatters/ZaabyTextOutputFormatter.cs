namespace Zaaby.AspNetCore.Formatters;

public class ZaabyTextOutputFormatter : TextOutputFormatter
{
    private readonly ITextSerializer _textSerializer;

    public ZaabyTextOutputFormatter(MediaTypeHeaderValue mediaTypeHeaderValue, ITextSerializer textSerializer)
    {
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
        SupportedMediaTypes.Add(mediaTypeHeaderValue);
        _textSerializer = textSerializer;
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding) =>
        await context.HttpContext.Response.WriteAsync(
            _textSerializer.ToText(
                context.ObjectType!,
                context.Object));
}