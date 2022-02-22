namespace Zaaby.Client.Http.Formatter;

public class ZaabyHttpClientFormatter
{
    internal static HttpRequestMessage CreateHttpRequestMessage(HttpContent httpContent, string mediaType, string requestUri)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri)
        {
            Content = httpContent
        };
        httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
        httpRequestMessage.Headers.Add("Accept", mediaType);
        return httpRequestMessage;
    }
}