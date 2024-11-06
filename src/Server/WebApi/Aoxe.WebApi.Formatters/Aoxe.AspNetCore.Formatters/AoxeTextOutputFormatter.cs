namespace Aoxe.AspNetCore.Formatters;

public class AoxeTextOutputFormatter : TextOutputFormatter
{
    private readonly ITextSerializer _textSerializer;

    public AoxeTextOutputFormatter(
        MediaTypeHeaderValue mediaTypeHeaderValue,
        ITextSerializer textSerializer
    )
    {
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
        SupportedMediaTypes.Add(mediaTypeHeaderValue);
        _textSerializer = textSerializer;
    }

    public override async Task WriteResponseBodyAsync(
        OutputFormatterWriteContext context,
        Encoding selectedEncoding
    ) =>
        await context
            .HttpContext
            .Response
            .WriteAsync(_textSerializer.ToText(context.ObjectType!, context.Object));
}
