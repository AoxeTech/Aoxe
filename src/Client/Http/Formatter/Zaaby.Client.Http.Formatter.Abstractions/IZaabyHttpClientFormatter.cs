namespace Zaaby.Client.Http.Formatter.Abstractions;

public interface IZaabyHttpClientFormatter
{
    string MediaType { get;}
    HttpRequestMessage CreateHttpRequestMessage(string requestUri, object? message);
    Task<object?> GetResultAsync(Type returnType, HttpResponseMessage httpResponseMessage);
}