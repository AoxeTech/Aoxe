namespace Zaaby.AspNetCore.Formatters;

public class ZaabyTextInputFormatter : TextInputFormatter
{
    private readonly ITextSerializer _textSerializer;
    private readonly IStreamSerializerAsync? _streamSerializerAsync;

    public ZaabyTextInputFormatter(MediaTypeHeaderValue mediaTypeHeaderValue, ITextSerializer textSerializer)
    {
        SupportedEncodings.Add(UTF8EncodingWithoutBOM);
        SupportedEncodings.Add(UTF16EncodingLittleEndian);
        SupportedMediaTypes.Add(mediaTypeHeaderValue);
        _textSerializer = textSerializer;
        _streamSerializerAsync = textSerializer as IStreamSerializerAsync;
    }

    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context,
        Encoding encoding) =>
        _streamSerializerAsync is null
            ? await InputFormatterResult.SuccessAsync(
                _textSerializer.FromBytes(context.ModelType, await context.HttpContext.Request.Body.ReadToEndAsync())!)
            : await InputFormatterResult.SuccessAsync(
                (await _streamSerializerAsync.FromStreamAsync(context.ModelType, context.HttpContext.Request.Body))!);
}