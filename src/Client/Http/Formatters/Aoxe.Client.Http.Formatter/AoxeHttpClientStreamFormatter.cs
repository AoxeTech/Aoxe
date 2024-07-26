namespace Aoxe.Client.Http.Formatter;

public class AoxeHttpClientStreamFormatter : AoxeHttpClientFormatter
{
    private readonly IStreamSerializer _serializer;
    public string MediaType { get; }

    public AoxeHttpClientStreamFormatter(AoxeClientFormatterOptions options)
    {
        _serializer = options.Serializer;
        MediaType = options.MediaType;
    }

    public override HttpRequestMessage CreateHttpRequestMessage(string requestUri, object? message)
    {
        var httpContent = new StreamContent(_serializer.ToStream(message));
        return CreateHttpRequestMessage(httpContent, MediaType, requestUri);
    }

    public override async Task<object?> GetResultAsync(
        Type returnType,
        HttpResponseMessage httpResponseMessage
    )
    {
        var result = await httpResponseMessage.Content.ReadAsStreamAsync();
        var type =
            returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>)
                ? returnType.GenericTypeArguments[0]
                : returnType;
        if (httpResponseMessage.IsSuccessStatusCode)
            return _serializer.FromStream(type, result);

        var aoxeError = _serializer.FromStream<AoxeError>(result)!;
        throw new AoxeException(aoxeError.Message)
        {
            Id = aoxeError.Id,
            Code = aoxeError.Code,
            ThrowTime = aoxeError.ThrowTime,
            Source = aoxeError.Source
        };
    }
}
