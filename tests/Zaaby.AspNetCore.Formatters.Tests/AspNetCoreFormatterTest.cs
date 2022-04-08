namespace Zaaby.AspNetCore.Formatters.Tests;

public partial class FormatterTest
{
    private readonly TestServer _server;

    public FormatterTest()
    {
        _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
    }

    private static HttpRequestMessage CreateHttpRequestMessage(HttpContent httpContent, string mediaType)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Values/Post")
        {
            Content = httpContent
        };
        httpRequestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mediaType);
        httpRequestMessage.Headers.Add("Accept", mediaType);

        return httpRequestMessage;
    }
}