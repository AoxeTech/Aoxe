namespace Zaaby.Client.Http.Formatter;

public class ZaabyHttpClientStreamFormatter : ZaabyHttpClientFormatter, IZaabyHttpClientFormatter
{
    private readonly IStreamSerializer _serializer;
    public string MediaType { get; }

    public ZaabyHttpClientStreamFormatter(ZaabyClientFormatterOptions options)
    {
        _serializer = options.Serializer;
        MediaType = options.MediaType;
    }

    public override HttpRequestMessage CreateHttpRequestMessage(string requestUri, object? message)
    {
        var httpContent = new StreamContent(_serializer.ToStream(message));
        return CreateHttpRequestMessage(httpContent, MediaType, requestUri);
    }

    public override async Task<object?> GetResultAsync(Type returnType, HttpResponseMessage httpResponseMessage)
    {
        var result = await httpResponseMessage.Content.ReadAsStreamAsync();
        var type = returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>)
            ? returnType.GenericTypeArguments[0]
            : returnType;
        if (httpResponseMessage.IsSuccessStatusCode)
            return _serializer.FromStream(type, result);

        var zaabyError = _serializer.FromStream<ZaabyError>(result)!;
        throw new ZaabyException(zaabyError.Message, zaabyError.StackTrace)
        {
            Id = zaabyError.Id,
            Code = zaabyError.Code,
            ThrowTime = zaabyError.ThrowTime,
            Source = zaabyError.Source
        };
    }
}