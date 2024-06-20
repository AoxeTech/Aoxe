namespace Aoxe.Client.Http.Formatter;

public abstract class AoxeHttpClientFormatter
{
    internal static HttpRequestMessage CreateHttpRequestMessage(
        HttpContent httpContent,
        string mediaType,
        string requestUri
    )
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri)
        {
            Content = httpContent
        };
        httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
        httpRequestMessage.Headers.Add("Accept", mediaType);
        return httpRequestMessage;
    }

    public abstract HttpRequestMessage CreateHttpRequestMessage(string requestUri, object? message);
    public abstract Task<object?> GetResultAsync(
        Type returnType,
        HttpResponseMessage httpResponseMessage
    );
}
