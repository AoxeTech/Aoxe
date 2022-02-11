using System.Text;
using Zaabee.Serializer.Abstractions;
using Zaaby.Client.Http.Formatter.Abstractions;
using Zaaby.Shared;

namespace Zaaby.Client.Http.Formatter.Jil;

public class ZaabyHttpClientFormatter : IZaabyHttpClientFormatter
{
    private readonly ITextSerializer _serializer;

    public ZaabyHttpClientFormatter(ITextSerializer serializer)
    {
        _serializer = serializer;
    }

    public string MediaType { get; set; } = "application/x-jil";

    public HttpRequestMessage CreateHttpRequestMessage(string requestUri, object? message)
    {
        var httpContent = new StringContent(_serializer.ToText(message), Encoding.UTF8, MediaType);
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri)
        {
            Content = httpContent
        };
        httpRequestMessage.Content.Headers.ContentType =
            new System.Net.Http.Headers.MediaTypeHeaderValue(MediaType);
        httpRequestMessage.Headers.Add("Accept", MediaType);
        return httpRequestMessage;
    }

    public async Task<object?> GetResultAsync(Type returnType, HttpResponseMessage httpResponseMessage)
    {
        var result = await httpResponseMessage.Content.ReadAsStringAsync();
        var type = returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>)
            ? returnType.GenericTypeArguments[0]
            : returnType;
        if (httpResponseMessage.IsSuccessStatusCode)
            return string.IsNullOrWhiteSpace(result)
                ? null
                : _serializer.FromText(type, result);

        var zaabyError = _serializer.FromText<ZaabyError>(result)!;
        throw new ZaabyException(zaabyError.Message, zaabyError.StackTrace)
        {
            Id = zaabyError.Id,
            Code = zaabyError.Code,
            ThrowTime = zaabyError.ThrowTime,
            Source = zaabyError.Source
        };
    }
}