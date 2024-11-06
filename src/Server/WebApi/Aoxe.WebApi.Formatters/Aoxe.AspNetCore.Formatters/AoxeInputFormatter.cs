namespace Aoxe.AspNetCore.Formatters;

public class AoxeInputFormatter : InputFormatter
{
    private readonly IBytesSerializer _bytesSerializer;
    private readonly IStreamSerializerAsync? _streamSerializerAsync;

    public AoxeInputFormatter(MediaTypeHeaderValue contentType, IBytesSerializer bytesSerializer)
    {
        SupportedMediaTypes.Add(contentType);
        _bytesSerializer = bytesSerializer;
        _streamSerializerAsync = bytesSerializer as IStreamSerializerAsync;
    }

    public override async Task<InputFormatterResult> ReadRequestBodyAsync(
        InputFormatterContext context
    ) =>
        _streamSerializerAsync is null
            ? await InputFormatterResult.SuccessAsync(
                _bytesSerializer.FromBytes(
                    context.ModelType,
                    await context.HttpContext.Request.Body.ReadToEndAsync()
                )!
            )
            : await InputFormatterResult.SuccessAsync(
                (
                    await _streamSerializerAsync.FromStreamAsync(
                        context.ModelType,
                        context.HttpContext.Request.Body
                    )
                )!
            );
}
