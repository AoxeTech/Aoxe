namespace Aoxe.Client.Http.Formatter;

public class AoxeHttpClientTextFormatter : AoxeHttpClientFormatter
{
    private readonly ITextSerializer _serializer;
    public string MediaType { get; }

    public AoxeHttpClientTextFormatter(AoxeClientFormatterOptions options)
    {
        if (options.Serializer is not ITextSerializer textSerializer)
            throw new ArgumentException("The Serializer is not ITextSerializer.");
        _serializer = textSerializer;
        MediaType = options.MediaType;
    }

    public override HttpRequestMessage CreateHttpRequestMessage(string requestUri, object? message)
    {
        var httpContent = new StringContent(_serializer.ToText(message), Encoding.UTF8, MediaType);
        return CreateHttpRequestMessage(httpContent, MediaType, requestUri);
    }

    public override async Task<object?> GetResultAsync(
        Type returnType,
        HttpResponseMessage httpResponseMessage
    )
    {
        var result = await httpResponseMessage.Content.ReadAsStringAsync();
        var type =
            returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>)
                ? returnType.GenericTypeArguments[0]
                : returnType;
        if (httpResponseMessage.IsSuccessStatusCode)
            return string.IsNullOrWhiteSpace(result) ? null : _serializer.FromText(type, result);

        var aoxeError = _serializer.FromText<AoxeError>(result)!;
        throw new AoxeException(aoxeError.Message)
        {
            Id = aoxeError.Id,
            Code = aoxeError.Code,
            ThrowTime = aoxeError.ThrowTime,
            Source = aoxeError.Source
        };
    }
}
